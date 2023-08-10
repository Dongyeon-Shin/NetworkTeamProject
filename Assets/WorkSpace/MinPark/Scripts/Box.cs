using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourPun, IExplosiveReactivable
{
    public ItemSetting item;
    public int index;
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    public void ExplosiveReact(Bomb bomb)
    {
        Hit(bomb);
    }

    private void Hit(Bomb bomb)
    {
        item = GetComponentInParent<ItemSetting>();
        // item이 비어있지 않으면 아이템 생성
        if (item.check[index] == 1)
            item.BoxHit(index, transform.position, bomb);
        PhotonNetwork.Destroy(gameObject);
    }
}
