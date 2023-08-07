using System.Collections;
using System.Collections.Generic;
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
    public int ExplosivePower { set { explosivePower = value; } }
    private LayerMask unPenetratedObjectsLayerMask;
    private BoxCollider bombCollider;

    private void Awake()
    {
        bombCollider = GetComponent<BoxCollider>();
        // TODO: 지금 외벽이 Box 레이어고 상자가 Default 레이어로 바뀌어 있는듯?
        unPenetratedObjectsLayerMask = LayerMask.GetMask("Box") | LayerMask.GetMask ("Bomb");
    }

    private void OnEnable()
    {

        bombCollider.enabled = true;
        foreach (Transform t in sparkParticle)
        {
            t.localScale = new Vector3(2f, 2f, 2f);
        }
        StartCoroutine(LightTheFuseRoutine());
    }

    IEnumerator LightTheFuseRoutine()
    {
        WaitForSeconds waitASecond = new WaitForSeconds(1);
        for (int i = 1; i <= fuseLength; i++)
        {
            sparkParticle[0].localScale = new Vector3(i, i, i);
            sparkParticle[1].localScale = new Vector3(i, i, i);
            sparkParticle[2].localScale = new Vector3(i, i, i);
            sparkParticle[3].localScale = new Vector3(i, i, i);
            yield return waitASecond;
        }
        bombCollider.enabled = false;
        yield return StartCoroutine(ExplodeRoutine());
    }

    IEnumerator ExplodeRoutine()
    {
        Explode(0, Vector3.zero);
        CheckObjectsInExplosionRange(Vector3.forward);
        CheckObjectsInExplosionRange(Vector3.back);
        CheckObjectsInExplosionRange(Vector3.right);
        CheckObjectsInExplosionRange(Vector3.left);
        yield return null;
        GameManager.Resource.Destroy(gameObject);
        GameManager.Event.PostNotification(EventType.Explode, this);
    }

    private void CheckObjectsInExplosionRange(Vector3 direction)
    {

        RaycastHit[] objectsInRange = Physics.RaycastAll(transform.position, direction, explosivePower);
        int explosionRange = 0;
        if (objectsInRange.Length == 0)
        {
            Explode(explosivePower, direction);
            return;
        }
        foreach (RaycastHit raycastHit in objectsInRange)
        {
            IExplosiveReactivable reactivableObject = raycastHit.collider.GetComponent<IExplosiveReactivable>();
            if (reactivableObject != null)
            {
                LayerMask reactivableObjectLayerMask = (1 << raycastHit.collider.gameObject.layer);
                reactivableObject.ExplosiveReact();
                if ((reactivableObjectLayerMask & unPenetratedObjectsLayerMask) > 0)
                {
                    Explode(explosionRange, direction);
                    return;
                }
                else if (reactivableObjectLayerMask == 1)
                {
                    explosionRange++;
                    Explode(explosionRange, direction);
                    return;
                }
                explosionRange++;
            }
        }
        Explode(explosionRange, direction);
    }

    private void Explode(int explosionRange, Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            GameManager.Resource.Instantiate(Resources.Load("Particle/ExplosionParticle"), transform.position, transform.rotation);
            return;
        }
        Vector3 position = transform.position;
        for (int i = 0; i < explosionRange; i++)
        {
            position += direction;
            GameManager.Resource.Instantiate(Resources.Load("Particle/ExplosionParticle"), position, transform.rotation);
        }
    }

    public void ExplosiveReact()
    {
        StopAllCoroutines();
        bombCollider.enabled = false;
        StartCoroutine(ExplodeRoutine());
    }

}
