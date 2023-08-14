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

    public void ExplosiveReact(Bomb bomb)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        if(check)
            StartCoroutine(Hit(bomb));
    }

    IEnumerator Hit(Bomb bomb)
    {
        check=false;
        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<PassiveItem>().Bomb = bomb;
        }
        Debug.Log("?^?");
        Destroy(gameObject);
        yield return null;
    }

}
