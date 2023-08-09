using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviourPun, IExplosiveReactivable
{
    protected int coefficient;
    private Bomb bomb;
    public Bomb Bomb { set { bomb=value; } }

    public void ExplosiveReact(Bomb bomb)
    {
        if (this.bomb == bomb)
            return;
        Destroy(gameObject);
        //포톤 사용시 아래의 디스트로이 사용
        //Destroy();
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
