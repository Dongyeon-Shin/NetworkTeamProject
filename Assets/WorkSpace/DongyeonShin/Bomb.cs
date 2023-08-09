using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour, IExplosiveReactivable
{
    [SerializeField]
    [Range(0, 10)]
    private int fuseLength;
    [SerializeField]
    private Transform[] sparkParticle;
    [SerializeField]
    private int explosivePower;
    [SerializeField]
    private float explosionEffectContinuanceTime;
    public int ExplosivePower { set { explosivePower = value; } }
    private LayerMask unPenetratedObjectsLayerMask;
    private LayerMask boxLayerMask;
    private BoxCollider bombCollider;
    private bool readyToExplode;
    private Coroutine lightTheFuseRoutine;
    private GameScene gameScene;
    public GameScene GameScene { get { return gameScene; } set { gameScene = value; } }

    private void Awake()
    {
        bombCollider = GetComponent<BoxCollider>();
        unPenetratedObjectsLayerMask = 1 | LayerMask.GetMask ("Bomb");
        boxLayerMask = LayerMask.GetMask("Box");
    }

    private void OnEnable()
    {
        bombCollider.enabled = true;
        foreach (Transform t in sparkParticle)
        {
            t.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    IEnumerator LightTheFuseRoutine()
    {
        WaitForSeconds waitASecond = new WaitForSeconds(1);
        readyToExplode = false;
        for (int i = 1; i <= fuseLength; i++)
        {
            sparkParticle[0].localScale = new Vector3(i, i, i);
            sparkParticle[1].localScale = new Vector3(i, i, i);
            sparkParticle[2].localScale = new Vector3(i, i, i);
            sparkParticle[3].localScale = new Vector3(i, i, i);
            yield return waitASecond;
        }
        bombCollider.enabled = false;
        readyToExplode = true;
    }

    public IEnumerator ExplodeRoutine()
    {
        lightTheFuseRoutine = StartCoroutine(LightTheFuseRoutine());
        yield return new WaitUntil(()=>  readyToExplode);
        StartCoroutine(ActualExplodeRoutine(0, Vector3.zero));
        CheckObjectsInExplosionRange(Vector3.forward);
        CheckObjectsInExplosionRange(Vector3.back);
        CheckObjectsInExplosionRange(Vector3.right);
        CheckObjectsInExplosionRange(Vector3.left);
        yield return null;
        GameManager.Resource.Destroy(gameObject);
    }

    private void CheckObjectsInExplosionRange(Vector3 direction)
    {

        RaycastHit[] objectsInRange = Physics.RaycastAll(transform.position + new Vector3(0f, 0.5f, 0f), direction, explosivePower);
        if (objectsInRange.Length == 0)
        {
            StartCoroutine(ActualExplodeRoutine(explosivePower, direction));
            return;
        }
        foreach (RaycastHit raycastHit in objectsInRange)
        {
            IExplosiveReactivable reactivableObject = raycastHit.collider.GetComponent<IExplosiveReactivable>();
            if (reactivableObject != null)
            {
                LayerMask reactivableObjectLayerMask = (1 << raycastHit.collider.gameObject.layer);
                if (gameScene != null)
                {
                    gameScene.RequestExplosiveReaction(reactivableObject, this);
                }   
                if ((reactivableObjectLayerMask & unPenetratedObjectsLayerMask) > 0)
                {
                    if (direction.z > 0)
                    {
                        StartCoroutine(ActualExplodeRoutine(raycastHit.transform.position.z - transform.position.z -1, direction));
                        return;
                    }
                    else if (direction.z < 0)
                    {
                        StartCoroutine(ActualExplodeRoutine(transform.position.z - raycastHit.transform.position.z - 1, direction));
                        return;
                    }
                    else if (direction.x > 0)
                    {
                        StartCoroutine(ActualExplodeRoutine(raycastHit.transform.position.x - transform.position.x - 1, direction));
                        return;
                    }
                    else
                    {
                        StartCoroutine(ActualExplodeRoutine(transform.position.x - raycastHit.transform.position.x - 1, direction));
                        return;
                    }
                }
                else if ((reactivableObjectLayerMask & boxLayerMask) > 0)
                {
                    if (direction.z > 0)
                    {
                        StartCoroutine(ActualExplodeRoutine(raycastHit.transform.position.z - transform.position.z, direction));
                        return;                                                       
                    }                                                                 
                    else if (direction.z < 0)                                         
                    {
                        StartCoroutine(ActualExplodeRoutine(transform.position.z - raycastHit.transform.position.z, direction));
                        return;                                                       
                    }                                                                 
                    else if (direction.x > 0)                                         
                    {
                        StartCoroutine(ActualExplodeRoutine(raycastHit.transform.position.x - transform.position.x, direction));
                        return;                                                       
                    }                                                                 
                    else                                                              
                    {
                        StartCoroutine(ActualExplodeRoutine(transform.position.x - raycastHit.transform.position.x, direction));
                        return;
                    }
                }
            }
        }
        StartCoroutine(ActualExplodeRoutine(explosivePower, direction));
    }

    private IEnumerator ActualExplodeRoutine(float explosionRange, Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            GameManager.Resource.Instantiate(Resources.Load("Particle/ExplosionParticle"), transform.position, transform.rotation);
            Coroutine checkAftermathOfExplosion = StartCoroutine(CheckAftermathOfExplosionRoutine(explosionRange, direction));
            yield return new WaitForSeconds(2.5f);
            StopCoroutine(checkAftermathOfExplosion);
        }
        else
        {
            Vector3 position = transform.position;
            for (int i = 0; i < explosionRange; i++)
            {
                position += direction;
                GameManager.Resource.Instantiate(Resources.Load("Particle/ExplosionParticle"), position, transform.rotation);
            }
            Coroutine checkAftermathOfExplosion = StartCoroutine(CheckAftermathOfExplosionRoutine(explosionRange, direction));
            yield return new WaitForSeconds(2.5f);
            StopCoroutine(checkAftermathOfExplosion);
        }
    }

    private IEnumerator CheckAftermathOfExplosionRoutine(float explosionRange, Vector3 direction)
    {
        while (true)
        {
            RaycastHit[] objectsInRange = Physics.RaycastAll(transform.position + new Vector3(0f, 0.5f, 0f), direction, explosionRange);
            foreach (RaycastHit raycastHit in objectsInRange)
            {
                IExplosiveReactivable reactivableObject = raycastHit.collider.GetComponent<IExplosiveReactivable>();
                if (reactivableObject != null)
                {
                    if (gameScene != null)
                    {
                        gameScene.RequestExplosiveReaction(reactivableObject, this);
                    }
                }
                yield return null;
            }
            yield return null;

        }
    }

    public void ExplosiveReact(Bomb bomb)
    {
        StopCoroutine(lightTheFuseRoutine);
        bombCollider.enabled = false;
        readyToExplode = true;
    }
}
