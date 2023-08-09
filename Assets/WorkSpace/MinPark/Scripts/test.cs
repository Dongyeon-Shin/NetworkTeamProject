using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class test : MonoBehaviourPunCallbacks
{
    public GameObject item;
    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();
            item.AddComponent<ItemSetting>();
        }
    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        DebugGameStart();
    }

    private void DebugGameStart()
    {
        float angularStart = (360.0f / 8f) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        Vector3 position = new Vector3(-3, 0.0f, 4);
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", position, rotation);
    }
}
