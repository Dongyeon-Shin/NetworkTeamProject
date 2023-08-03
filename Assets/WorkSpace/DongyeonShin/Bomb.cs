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
    private LayerMask penetratedLayerMask;
    private LayerMask unPenetratedLayerMask;

    private void Awake()
    {
        penetratedLayerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Item");
        unPenetratedLayerMask = LayerMask.GetMask("Box") | LayerMask.GetMask ("Bomb");
    }

    private void OnEnable()
    {
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
        yield return StartCoroutine(ExplodeRoutine());
        GameManager.Event.PostNotification(EventType.Explode, this);
        GameManager.Resource.Destroy(gameObject);
    }

    IEnumerator ExplodeRoutine()
    {
        Explode(0, Vector3.zero);
        CheckObjectsInExplosionRange(Vector3.forward);
        CheckObjectsInExplosionRange(Vector3.back);
        CheckObjectsInExplosionRange(Vector3.right);
        CheckObjectsInExplosionRange(Vector3.left);
        yield return null;
    }

    private void CheckObjectsInExplosionRange(Vector3 direction)
    {

        RaycastHit[] objectsInRange = Physics.RaycastAll(transform.position, direction, explosivePower);
        int explosionRange = 0;
        if (objectsInRange == null)
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
                if ((reactivableObjectLayerMask & unPenetratedLayerMask) > 0)
                {
                    break;
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
        for (int i = 0; i <= explosionRange; i++)
        {
            position += direction;
            GameManager.Resource.Instantiate(Resources.Load("Particle/ExplosionParticle"), position, transform.rotation);
        }
    }

    public void ExplosiveReact()
    {
        StartCoroutine(ExplodeRoutine());
    }
}
