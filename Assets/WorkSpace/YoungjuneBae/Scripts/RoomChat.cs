using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class RoomChat : MonoBehaviourPunCallbacks
{
    [SerializeField] RectTransform content;
    [SerializeField] Text chatPrefab;
    [SerializeField] public InputField input;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && input.isFocused == false)
        {
            input.ActivateInputField();
        }
    }
    public void SendChat()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            
            string strMessage = PhotonNetwork.LocalPlayer.NickName + " : " + input.text;

            photonView.RPC("ReceiveMsg", RpcTarget.All, strMessage);
            input.text = "";
        }
    }

    public void OutPutMsg(string strMessage)
    {
        Text text = Instantiate(chatPrefab, content);

        text.text = strMessage;
    }
    [PunRPC]
    public void ReceiveMsg(string strMessage)
    {
        OutPutMsg(strMessage);
    }

    public void InputSelect()
    {
        input.Select();
    }
}
