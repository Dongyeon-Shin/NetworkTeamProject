using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] Text roomName;
    [SerializeField] Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    private RoomInfo info;
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name;
        currentPlayer.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        joinRoomButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers;
    }
    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(roomName.text);
    }
}
