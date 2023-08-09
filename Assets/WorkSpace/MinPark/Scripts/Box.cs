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
        Hit(bomb);
    }

    private void Hit(Bomb bomb)
    {
        item = GetComponentInParent<ItemSetting>();
        // item�� ������� ������ ������ ����
        if (item.check[index] == 1)
            item.BoxHit(index, transform.position, bomb);
        PhotonNetwork.Destroy(gameObject);
    }
}
