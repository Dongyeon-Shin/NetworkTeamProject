using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : PassiveItem
{
    protected override void CoeffiCient()
    {
        coefficient = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PlayerStat stat = other.GetComponent<PlayerStat>();
            stat.Bomb = coefficient;
            stat.StatRenewal();
            stat.ItemInterfaceSet();
            gameScene.ItemDestroy(gameObject);
        }
    }

}
