using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviourPun, IExplosiveReactivable
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
    private GameScene gameScene;
    public GameScene GameScene { get { return gameScene; } set { gameScene = value; } }
    [SerializeField]
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }
    public int ExplosivePower { set { explosivePower = value; } }

    private LayerMask unPenetratedObjectsLayerMask;
    private LayerMask bombLayerMask;
    private LayerMask boxLayerMask;
    private BoxCollider boxCollider;
    private SphereCollider explodCollider;
    private bool readyToExplode;
    private Coroutine lightTheFuseRoutine;
    private MeshRenderer bombRenderer;
    private AudioSource explodClip;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        explodCollider = GetComponent<SphereCollider>();
        explodClip = GetComponent<AudioSource>();
        bombLayerMask = LayerMask.GetMask("Bomb");
        boxLayerMask = LayerMask.GetMask("Box");
        unPenetratedObjectsLayerMask = 1 | bombLayerMask;
        bombRenderer = GetComponentInChildren<MeshRenderer>();

    }

    private void OnEnable()
    {
        boxCollider.isTrigger = true;
        explodCollider.enabled = true;
        sparkParticle[0].gameObject.SetActive(true);
        sparkParticle[1].gameObject.SetActive(true);
        sparkParticle[2].gameObject.SetActive(true);
        sparkParticle[3].gameObject.SetActive(true);
        bombRenderer.enabled = true;
        foreach (Transform t in sparkParticle)
        {
            t.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    private void Start()
    {
        gameScene.RegisterBombID(this);
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
        explodCollider.enabled = false;
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
        sparkParticle[0].gameObject.SetActive(false);
        sparkParticle[1].gameObject.SetActive(false);
        sparkParticle[2].gameObject.SetActive(false);
        sparkParticle[3].gameObject.SetActive(false);
        bombRenderer.enabled = false;
        GameManager.Resource.Destroy(gameObject, explosionEffectContinuanceTime);
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
              if (PhotonNetwork.IsMasterClient)
              {
                  gameScene.RequestExplosiveReaction(reactivableObject, iDNumber, ((reactivableObjectLayerMask & bombLayerMask) > 0));
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
            explodClip.Play();   // 오디오 소스
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
                  if (PhotonNetwork.IsMasterClient)
                  {
                      gameScene.RequestExplosiveReaction(reactivableObject, iDNumber, (((1 << raycastHit.collider.gameObject.layer) & bombLayerMask) > 0));
                  }
                }
                yield return null;
            }
            yield return null;

        }
    }

    public void ExplosiveReact(Bomb bomb)
    {
        Debug.Log("bombCheck");
        StopCoroutine(lightTheFuseRoutine);
        explodCollider.enabled = false;
        readyToExplode = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            boxCollider.isTrigger = false;
        }
    }
}
