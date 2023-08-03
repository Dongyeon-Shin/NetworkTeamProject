using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject item;

    private void Hit()
    {
        // item�� ������� ������ ������ ����
        if(item != null)
            Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Hit();
    }
}
