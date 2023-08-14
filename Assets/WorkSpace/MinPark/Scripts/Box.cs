using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourPun, IExplosiveReactivable
{
    private BoxCollider collider;
    public GameObject item;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    [SerializeField]
    private int iDNumber;   
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    public void ExplosiveReact(int bombIDNumber)
    {
        Hit(bombIDNumber);
    }

    private void Hit(int bombIDNumber)
    {
        Debug.Log("hit");
        if(item != null)
        {
            //Instantiate(item,transform.position,Quaternion.Euler(0,0,0)).GetComponent<PassiveItem>().Bomb=bomb;
        }
        Destroy(gameObject);
    }
}
