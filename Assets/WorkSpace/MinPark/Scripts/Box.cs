using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourPun, IExplosiveReactivable
{
    public ItemSetting item;
    public int index;
    [SerializeField]
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    public void ExplosiveReact(Bomb bomb)
    {
        Hit(bomb);
    }

    private void Hit(Bomb bomb)
    {
        item = GetComponentInParent<ItemSetting>();
        // item�� ������� ������ ������ ����
        PhotonNetwork.Destroy(gameObject);
    }
}
