using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviourPun, IExplosiveReactivable
{
    protected int coefficient;

    public void ExplosiveReact()
    {
        Destroy(gameObject);
    }
    protected void Destroy()
    {
        photonView.RPC("RequestItemDestroy", RpcTarget.All);
    }
    [PunRPC]
    private void RequestItemDestroy()
    {
        GameManager.Resource.Destroy(gameObject);
    }
    protected abstract void CoeffiCient();

    private void Start()
    {
        CoeffiCient();
    }
}
