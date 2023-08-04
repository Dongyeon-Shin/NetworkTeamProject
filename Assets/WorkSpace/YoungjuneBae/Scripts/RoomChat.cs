using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomChat : MonoBehaviourPunCallbacks
{
    [SerializeField] Text chatLog;
    [SerializeField] InputField input;

    ScrollRect scrollRect;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scrollRect=GameObject.FindObjectOfType<ScrollRect>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)&&!input.isFocused) 
        {
            SendChat();
        }
    }

    public void SendChat()
    {
        if (input.text.Equals(""))
            return;
        string msg=string.Format("[{0}] : {1}",PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg",RpcTarget.Others,msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
    }
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text =msg+"\n";
        scrollRect.verticalNormalizedPosition = 0.0f;
    }
}
