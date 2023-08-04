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

    string playerID;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings();//네트워크 연결 테스트용
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && input.isFocused == false)
        {
            input.ActivateInputField();
        }
    }
    public override void OnConnectedToMaster()//네트워크 연결 테스트용    
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 5;

        int nRandomKey = Random.Range(0, 100);

        playerID = "user" + nRandomKey;

        PhotonNetwork.LocalPlayer.NickName = playerID;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
    }

    public void SendChat()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string strMessage = playerID + " : " + input.text;

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
}
