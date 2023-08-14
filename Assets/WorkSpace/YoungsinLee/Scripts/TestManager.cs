using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform startPosition;
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
        Vector3 position = new Vector3(startPosition.transform.position.x, 0.0f, startPosition.transform.position.z);
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        PhotonNetwork.Instantiate("Player/Player_Reindeer2", position, rotation);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("너 나가진거야");
        PhotonNetwork.LoadLevel("LobbyScene");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    

}
