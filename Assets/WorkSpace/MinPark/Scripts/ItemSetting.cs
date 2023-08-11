using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviourPun
{
    // 박스 갯수 58개
    private Items items;
    public GameObject[] itemArray;
    public Bomb bomb;
    private int itemCount = 30;
    public int[] check;  // 아이템이 들어있는지 확인용 0 false 1 true
    public int[] item;   // 무슨 아이템이 들었는지 확인용
    public GameScene gameScene;

    // 디버그모드
    int players=2;

    private void Start()
    {
        ItemCreate();
    }

    public void ItemSettingConnect(GameScene gameScene)
    {
        this.gameScene = gameScene;
    }

    public void ItemCreate()
    {
        items = transform.GetComponent<Items>();
        itemArray = new GameObject[items.item.Length];
        int i = 0;
        foreach (GameObject item in items.item)
        {
            itemArray[i++] = item;
        }
        // 방장만 아이템을 생성
        if (PhotonNetwork.IsMasterClient)
        {
            ArraySetting();
            StartCoroutine(ItemSetDelay());
            gameScene.ItemCreate();
        }
        else
        {
            gameScene.ItemCreate();
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

    IEnumerator ItemSetDelay()
    {
        yield return new WaitWhile(() => PhotonNetwork.PlayerList.Length != players);
        ItemSet();
    }
    public void ItemSet()
    {
        int index = 0;
        foreach(Transform trans in transform)
        {
            if (itemCount < 1)
            {
                gameScene.ArrayCopy(check, item);
                return;
            }
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
                }
            }
            index++;
        }
        if (itemCount > 0)
            ItemSet();
    }
}
