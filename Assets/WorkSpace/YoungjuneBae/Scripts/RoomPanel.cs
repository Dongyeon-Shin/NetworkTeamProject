using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CustomProperty;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject[] playerSpace;    
    [SerializeField] Button startButton;
    [SerializeField] List<RenderTexture> renderTextures;
    public enum PlayerType { Color1, Color2, Color3, Color4 }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerEntry();
    }

    public void PlayerPropertyUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (changedProps.ContainsKey(PlayerNumbering.RoomPlayerIndexedProp))
        {
            UpdatePlayerEntry();
        }
        ReadyCheck();
    }

    private void UpdatePlayerEntry()
    {
        HashTable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(cp["SoloMode"]);
        Debug.Log(cp["TeamMode"]);
        for (int i = 0; i < playerSpace.Length; i++)
        {
            playerSpace[i].SetActive(true);
            playerSpace[i].transform.parent.GetComponentInChildren<Text>().text = "";
            playerSpace[i].transform.parent.GetComponent<Image>().color = new Color(145 / 255f, 250 / 255f, 220.255f);
        }

        int playerCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerSpace[playerCount].SetActive(false);
            PlayerType key = (PlayerType)player.GetPlayerNumber();
            playerSpace[playerCount].transform.parent.GetComponentInChildren<RawImage>().texture = renderTextures[(int)key];
            playerSpace[playerCount].transform.parent.GetComponentInChildren<Text>().text = player.NickName;
            if(player.GetReady())
            {
                playerSpace[playerCount].GetComponentInParent<Image>().color = Color.red;
            }
            else
            {
                playerSpace[playerCount].transform.parent.GetComponent<Image>().color = new Color(145 / 255f, 250 / 255f, 220.255f);
            }
            playerCount++;
        }
        
    }
    public void ReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    public void LeaveRoom()//�涰����
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()//���� ���� ��ư
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void Ready()//�غ� ��ư
    {
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;
        PhotonNetwork.LocalPlayer.SetReady(ready);
    }
}
