using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PassiveItem
{
    protected override void CoeffiCient()
    {
        coefficient = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            other.GetComponent<PlayerStat>().Speed = coefficient;
            other.GetComponent<PlayerStat>().ItemInterfaceSet();
            
            Destroy(gameObject);
        }
    }
    public void GameSceneSet(GameScene gameScene)
    {
        this.gameScene = gameScene;
    }
}
