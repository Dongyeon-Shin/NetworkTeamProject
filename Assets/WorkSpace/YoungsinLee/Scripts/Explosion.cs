using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public Renderer start;
    [SerializeField] public Renderer mid;
    [SerializeField] public Renderer end;

    public void SetActiveRenderer(Renderer renderer)
    {
        start.enabled = renderer == start;
        mid.enabled = renderer == mid;
        end.enabled = renderer == end;
    }

    public void SetDirextion(Vector3 dir) 
    {
        float angle = Mathf.Atan2(dir.z, dir.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestrotyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }

}
