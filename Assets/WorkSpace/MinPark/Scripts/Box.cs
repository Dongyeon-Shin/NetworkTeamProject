using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IExplosiveReactivable
{
    public ItemSetting item;
    public int index;

    public void ExplosiveReact()
    {
        Hit();
    }

    private void Hit()
    {
        item = GetComponentInParent<ItemSetting>();
        // item�� ������� ������ ������ ����
        if (item.check[index]==1)
            PhotonNetwork.Instantiate($"Item/Item[item.item[index]]", transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
