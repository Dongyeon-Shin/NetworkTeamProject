//using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    // 박스 갯수 88개
    private Items items;
    private GameObject[] itemArray;
    private int itemCount=30;

    private void Start()
    {
        // 방장만 아이템을 생성
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
            // 부모 트랜스폼과 같으면 건너뛴다
            if (trans == transform)
                continue;
            if(trans.GetComponent<Box>().item == null)
            {
                // 3분의 1확률로 아이템 배치
                if(Random.Range(0, 3) == 0)
                {
                    // 배치할 아이템을 선택
                    trans.GetComponent<Box>().item = itemArray[Random.Range(0, itemArray.Length)];
                    itemCount -= 1;
                }
            }
        }
        if (itemCount > 0)
            ItemSet();
    }
}
