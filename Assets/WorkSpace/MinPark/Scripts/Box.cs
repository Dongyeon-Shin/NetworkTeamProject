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
        // item�� ������� ������ ������ ����
        if(item != null)
            Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
