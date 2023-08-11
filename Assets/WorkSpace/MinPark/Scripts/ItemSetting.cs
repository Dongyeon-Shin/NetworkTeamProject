using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviourPun
{
    // �ڽ� ���� 58��
    private Items items;
    public GameObject[] itemArray;
    public Bomb bomb;
    private int itemCount = 30;
    public int[] check;  // �������� ����ִ��� Ȯ�ο� 0 false 1 true
    public int[] item;   // ���� �������� ������� Ȯ�ο�
    public GameScene gameScene;

    // ����׸��
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
        // ���常 �������� ����
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
            // �θ� Ʈ�������� ������ �ǳʶڴ�
            if (trans == transform)
                continue;
            if (check[index]==0)
            {
                // 3���� 1Ȯ���� ������ ��ġ
                if(Random.Range(0, 3) == 0)
                {
                    // ��ġ�� �������� ����
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
