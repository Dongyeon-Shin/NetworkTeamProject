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
            //방이 사라질 예정이면+ 방이 비공개가 되었으면 + 방이 닫혔으면
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                if (roomDictionary.ContainsKey(info.Name))
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }
            //방이 자료구조에 존재했다면 (그냥 무조건 이름이 있었던 방이면 최신으로)
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            //방이 생겼으면
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
