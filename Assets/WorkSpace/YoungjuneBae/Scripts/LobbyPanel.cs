using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    [SerializeField] public GameObject roomEnterFail;

    Dictionary<string, RoomInfo> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomInfo>();
    }

    private void OnDisable()
    {
        roomDictionary.Clear();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        //Clear room list
        for (int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }
        //Update room info
        foreach (RoomInfo info in roomList)
        {
            //���� ����� �����̸�+ ���� ������� �Ǿ����� + ���� ��������
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                if (roomDictionary.ContainsKey(info.Name))
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }
            //���� �ڷᱸ���� �����ߴٸ� (�׳� ������ �̸��� �־��� ���̸� �ֽ�����)
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            //���� ��������
            else
            {
                roomDictionary.Add(info.Name, info);
            }
        }

        //Create room list
        foreach (RoomInfo info in roomDictionary.Values)
        {
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            entry.SetRoomInfo(info);
        }
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
