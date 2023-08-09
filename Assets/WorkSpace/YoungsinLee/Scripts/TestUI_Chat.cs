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

public class TestUI_Chat : MonoBehaviourPunCallbacks
{
    [SerializeField] RectTransform content;
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField input;
    [SerializeField] PlayerMove player;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && input.isFocused == false)
        {
            SendChat();
        }
    }

    public void SendChat()
    {
        if (input.text.Equals(""))
            return;
        string strMessage =" : " + input.text;
        photonView.RPC("ReceiveMsg", RpcTarget.All, strMessage);
        input.text = "";
        OnChatOff();
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

    public void OnChat()
    {
        input.gameObject.SetActive(true);
        input.Select();
    }

    public void OnChatOff()
    {
        player.IsChatting = false;
        input.gameObject.SetActive(false);
    }
}
