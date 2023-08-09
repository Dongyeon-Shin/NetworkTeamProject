using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourPun, IExplosiveReactivable
{
    public ItemSetting item;
    public int index;

    public void ExplosiveReact(Bomb bomb)
    {
        Hit();
    }

    private void Hit()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            item = GetComponentInParent<ItemSetting>();
            // item이 비어있지 않으면 아이템 생성
            if (item.check[index] == 1)
                item.BoxHit(index, transform.position);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC]
    private void ItemCreate()
    {
        Instantiate(item.itemArray[item.item[index]], transform.position, Quaternion.Euler(0, 0, 0));
    }
}
