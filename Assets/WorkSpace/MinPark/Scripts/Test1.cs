using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviourPun
{
    private void Start()
    {
        StartCoroutine(test());
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        photonView.RPC("asdsaf", RpcTarget.AllViaServer);
    }
    [PunRPC]
    private void asdsaf()
    {
        Debug.Log("CheckPoint1");
    }
}
