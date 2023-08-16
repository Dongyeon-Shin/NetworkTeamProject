using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourPun, IExplosiveReactivable
{
    private BoxCollider boxcollider;
    public GameObject item;
    private bool check=true;

    private void Awake()
    {
        boxcollider = GetComponent<BoxCollider>();
    }

    [SerializeField]
    private int iDNumber;   
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    public void ExplosiveReact(int bombCount)
    {
        if (check)
            Hit(bombCount);
    }

    private void Hit(int bombCount)
    {
        check=false;
        if (item != null)
        {
            GameObject ite = Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
            ite.GetComponent<PassiveItem>().SetBombNumber(bombCount);
        }
        Destroy(gameObject);
    }

}
