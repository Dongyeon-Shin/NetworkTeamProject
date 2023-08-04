using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IExplosiveReactivable
{
    public GameObject item;

    public void ExplosiveReact()
    {
        Hit();
    }

    private void Hit()
    {
        // item이 비어있지 않으면 아이템 생성
        if(item != null)
            Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
