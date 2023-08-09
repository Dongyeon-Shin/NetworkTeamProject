using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviourPunCallbacks
{
    // 박스 갯수 58개
    private Items items;
    public GameObject[] itemArray;
    public Bomb bomb;
    private int itemCount = 30;
    public int[] check;  // 아이템이 들어있는지 확인용 0 false 1 true
    public int[] item;   // 무슨 아이템이 들었는지 확인용
    public void ItemCreate()
    {
        // 방장만 아이템을 생성
        if (PhotonNetwork.IsMasterClient)
        {
            ArraySetting();
            ChildIndexSetting();
            items = transform.GetComponent<Items>();
            itemArray = new GameObject[items.item.Length];
            int i = 0;
            foreach (GameObject item in items.item)
            {
                itemArray[i++] = item;
            }
            ItemSet();
        }
    }
    private void ArraySetting()
    {
        check = new int[transform.childCount];
        item = new int[transform.childCount];
        for (int j = 0; j < transform.childCount; j++)
        {
            check[j] = 0;
            item[j] = 0;
        }
    }
    private void ChildIndexSetting()
    {
        int index = 0;
        foreach (Transform trans in transform)
        {
            if (trans == transform)
                continue;
            transform.GetChild(index).GetComponent<Box>().index = index;
            index++;
        }
    }
    public void BoxHit(int index, Vector3 position, Bomb bomb)
    {
        photonView.RPC("BoxItem", RpcTarget.AllViaServer, index, position,bomb);
    }
    [PunRPC]
    private void BoxItem(int index, Vector3 position, Bomb bomb)
    {
        Instantiate(itemArray[item[index]], position, Quaternion.Euler(0, 0, 0)).GetComponent<PassiveItem>().Bomb=bomb;
    }
    // 디버그 모드 전용
    public override void OnJoinedRoom()
    {
        StartCoroutine(DebugGameSetupDelay());
    }
    IEnumerator DebugGameSetupDelay()
    {
        yield return new WaitForSeconds(1f);
        ItemCreate();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public bool CheckRoomMaster()
    {
        return (PhotonNetwork.IsMasterClient);
    }
    public void ItemSet()
    {
        int index = 0;
        foreach(Transform trans in transform)
        {
            if (itemCount < 1)
                break;
            // 부모 트랜스폼과 같으면 건너뛴다
            if (trans == transform)
                continue;
            if (check[index]==0)
            {
                // 3분의 1확률로 아이템 배치
                if(Random.Range(0, 3) == 0)
                {
                    // 배치할 아이템을 선택
                    check[index] = 1;
                    item[index] = Random.Range(0, itemArray.Length);
                    itemCount -= 1;
                    Debug.Log($"index:{index} :{check[index]}: {item[index]}");
                }
            }
            index++;
        }
        if (itemCount > 0)
            ItemSet();
    }
}
