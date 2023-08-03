//using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    // �ڽ� ���� 88��
    private Items items;
    private GameObject[] itemArray;
    private int itemCount=30;

    private void Start()
    {
        // ���常 �������� ����
        //if (PhotonNetwork.IsMasterClient)
        items = GetComponent<Items>();
        itemArray = new GameObject[items.item.Length];
        int i = 0;
        foreach (GameObject item in items.item)
        {
            itemArray[i++] = item;
        }
        ItemSet();
    }
    public void ItemSet()
    {
        foreach(Transform trans in transform)
        {
            if (itemCount < 1)
                break;
            // �θ� Ʈ�������� ������ �ǳʶڴ�
            if (trans == transform)
                continue;
            if(trans.GetComponent<Box>().item == null)
            {
                // 3���� 1Ȯ���� ������ ��ġ
                if(Random.Range(0, 3) == 0)
                {
                    // ��ġ�� �������� ����
                    trans.GetComponent<Box>().item = itemArray[Random.Range(0, itemArray.Length)];
                    itemCount -= 1;
                }
            }
        }
        if (itemCount > 0)
            ItemSet();
    }
}
