using Photon.Chat.Demo;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerChat : MonoBehaviourPun
{
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField input;
    [SerializeField] GameObject popUpChat;

    private bool isChatting = false;
    public bool IsChatting { get { return isChatting; } set { isChatting = value; } }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    
    public void SendChat()
    {
        if (input.text.Equals(""))
            return;
        string strMessage = " : " + input.text;
        photonView.RPC("ReceiveMsg", RpcTarget.All, strMessage);
        input.text = "";
        photonView.RPC("PopUpChat", RpcTarget.All);
    }

    [PunRPC]
    public void ReceiveMsg(string strMessage)
    {
        chatText.text = strMessage;
    }

    [PunRPC]
    public void PopUpChat()
    {
        StartCoroutine(ChatRoutine());
    }

    IEnumerator ChatRoutine()
    {
        popUpChat.SetActive(true);
        yield return new WaitForSeconds(3f);
        popUpChat.SetActive(false);
    }


    private void OnChatting(InputValue value)
    {
        if (IsChatting == true)
        {
            SendChat();
            input.gameObject.SetActive(false);
            isChatting = false;
        }
        else
        {
            input.gameObject.SetActive(true);
            input.Select();
            isChatting = true;
        }
    }

}
