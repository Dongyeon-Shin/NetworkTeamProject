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

    public void ExplosiveReact(int bombIDNumber)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        if(check)
        Hit(bombIDNumber);
    }

    private void Hit(int bombIDNumber)
    {
        check=false;
        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<PassiveItem>().BombIDNumber = bombIDNumber;
        }
        Destroy(gameObject);
    }

}
