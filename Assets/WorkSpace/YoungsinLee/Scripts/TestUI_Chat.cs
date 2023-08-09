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
    [SerializeField] PlayerMove player;

    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void SendChat()
    {
        if (inputField.text.Equals(""))
            return;
        string strMessage = " : " + inputField.text;
        photonView.RPC("ReceiveMsg", RpcTarget.All, strMessage);
        inputField.text = "";
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
}
