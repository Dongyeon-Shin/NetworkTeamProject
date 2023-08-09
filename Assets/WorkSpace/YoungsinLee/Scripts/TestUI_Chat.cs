using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.InputSystem;
using Photon.Chat.Demo;

public class TestUI_Chat : MonoBehaviourPunCallbacks
{
    [SerializeField] RectTransform content;
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField input;
    private PlayerMove player;

    private void Awake()
    {
        player = GameManager.Resource.Load<PlayerMove>("Player/Player_Reindeer2");
    }
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void SendChat()
    {
        if (input.text.Equals(""))
            return;
        string strMessage =" : " + input.text;
        photonView.RPC("ReceiveMsg", RpcTarget.All, strMessage);
        input.text = "";
    }

    [PunRPC]
    public void ReceiveMsg(string strMessage)
    {
        OutPutMsg(strMessage);
    }

    public void OutPutMsg(string strMessage)
    {
        TMP_Text text = Instantiate(chatText);
        text.text = strMessage;
        text.transform.SetParent(content);
    }

    public void OnChatting(InputValue value)
    {
        if(player.IsChatting == false)
        {
            player.IsChatting = true;
            OnActive();
        }
        else if (player.IsChatting == true)
        {
            SendChat();
            player.IsChatting = false;
            input.gameObject.SetActive(false);
        }

    }
    public void OnActive()
    {
        input.gameObject.SetActive(true);
        input.Select();
    }
}
