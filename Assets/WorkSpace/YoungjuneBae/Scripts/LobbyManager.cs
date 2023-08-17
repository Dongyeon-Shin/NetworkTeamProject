using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaeProperty;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room, Setting}

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] LobbyPanel lobbyPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] SettingPanel settingPanel;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            OnConnectedToMaster();
        }
        else if (PhotonNetwork.InRoom)
        {
            OnJoinedRoom();
        }
        else if (PhotonNetwork.InLobby)
        {
            OnJoinedLobby();
        }
        else
            OnDisconnected(DisconnectCause.None);
    }

    //======================마스터서버 접속========================
    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }
    //======================마스터서버 접속해제=======================
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    //========================로비 메소드=============================
    public override void OnJoinedLobby()//로비로 입장
    {
        SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()//로비를 퇴장
    {
        SetActivePanel(Panel.Menu);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//방 리스트가 갱신될 때
    {
        lobbyPanel.UpdateRoomList(roomList);
    }
    //=========================방 관련 메소드==================================//
    public override void OnCreateRoomFailed(short returnCode, string message)   //방생성 실패
    {
        SetActivePanel(Panel.Menu);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)   //랜덤 방 입장 실패
    {

        SetActivePanel(Panel.Lobby);
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)    //방 입장 실패
    {
        SetActivePanel(Panel.Lobby);
        lobbyPanel.roomEnterFail.SetActive(true);
    }
    public override void OnJoinedRoom()         //방으로 입장
    {
        SetActivePanel(Panel.Room);

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnLeftRoom()           //방에서 나가기
    {
        roomPanel.selectMap.SetActive(false);
        roomPanel.waitMap.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = false;
        SetActivePanel(Panel.Menu);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  //플레이어 방 입장
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)   //플레이어 방 나가기
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)     //방장이 바뀔 때
    {
        roomPanel.ReadyCheck();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)//플레이어 프로퍼티가(레디상태) 변경되었을 때
    {
        roomPanel.PlayerPropertyUpdate(targetPlayer, changedProps);
    }
    //=============================================================================


    //=============방 상태를 열거로 정리
    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
        settingPanel.gameObject?.SetActive(panel == Panel.Setting);
    }

}
