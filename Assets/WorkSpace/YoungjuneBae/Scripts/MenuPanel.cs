using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HashTabel = ExitGames.Client.Photon.Hashtable;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject createRoom;
    [SerializeField] InputField roomNameInput;
    [SerializeField] InputField maxPlayerInput;
    [SerializeField] Toggle soloMode;
    [SerializeField] Toggle teamMode;
    [SerializeField] GameObject selectMode;
    

    private void OnEnable()
    {
        createRoom.SetActive(false);
    }
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void RandomRoomEnter()
    {
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        options.CustomRoomProperties = new HashTabel() { { "SoloMode", true }, { "TeamMode", false } };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
    }

    public void CreateRoomMenu()
    {
        createRoom.SetActive(true);
    }

    public void CreateRoom()
    {
        string roomName=roomNameInput.text;
        if(roomName =="")
        {
            roomName = $"Room {Random.Range(1000, 10000)}";
        }
        int maxPlayer = maxPlayerInput.text==""?4:int.Parse(maxPlayerInput.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 2, 4);

        RoomOptions options = new RoomOptions();
        options.CustomRoomProperties= new HashTabel() { { "SoloMode", soloMode.isOn }, { "TeamMode",teamMode.isOn } };
        options.MaxPlayers = maxPlayer;
        if(!soloMode.isOn&&!teamMode.isOn)
        {
            selectMode.SetActive(true);
            return;
        }
        PhotonNetwork.CreateRoom(roomName, options);
       
    }

    public void CreateCancel()
    {
        createRoom.SetActive(false);
    }
    public void soloModeToggle()
    {
        if(soloMode.isOn)
        {
            teamMode.isOn = false;
        }
    }
    public void TeamModeToggle()
    {
        if (teamMode.isOn)
        {
            soloMode.isOn = false;
        }
    }

}
