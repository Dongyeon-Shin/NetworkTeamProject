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

    //======================�����ͼ��� ����========================
    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }
    //======================�����ͼ��� ��������=======================
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    //========================�κ� �޼ҵ�=============================
    public override void OnJoinedLobby()//�κ�� ����
    {
        SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()//�κ� ����
    {
        SetActivePanel(Panel.Menu);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//�� ����Ʈ�� ���ŵ� ��
    {
        lobbyPanel.UpdateRoomList(roomList);
    }
    //=========================�� ���� �޼ҵ�==================================//
    public override void OnCreateRoomFailed(short returnCode, string message)   //����� ����
    {
        SetActivePanel(Panel.Menu);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)   //���� �� ���� ����
    {

        SetActivePanel(Panel.Lobby);
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)    //�� ���� ����
    {
        SetActivePanel(Panel.Lobby);
        lobbyPanel.roomEnterFail.SetActive(true);
    }
    public override void OnJoinedRoom()         //������ ����
    {
        SetActivePanel(Panel.Room);

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnLeftRoom()           //�濡�� ������
    {
        roomPanel.selectMap.SetActive(false);
        roomPanel.waitMap.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = false;
        SetActivePanel(Panel.Menu);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  //�÷��̾� �� ����
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)   //�÷��̾� �� ������
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)     //������ �ٲ� ��
    {
        roomPanel.ReadyCheck();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)//�÷��̾� ������Ƽ��(�������) ����Ǿ��� ��
    {
        roomPanel.PlayerPropertyUpdate(targetPlayer, changedProps);
    }
    //=============================================================================


    //=============�� ���¸� ���ŷ� ����
    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
        settingPanel.gameObject?.SetActive(panel == Panel.Setting);
    }

}
