using Photon.Chat.Demo;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPun
{
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField input;
    [SerializeField] GameObject popUpChat;
    [SerializeField] GameObject settingUI;
    [SerializeField] Button backButton;
    [SerializeField] Button exitButton;

    private PlayerStat stat;
    private Animator animator;


    private bool isChatting = false;
    private bool isSetting = false;
    public bool IsChatting { get { return isChatting; } set { isChatting = value; } }
    public bool IsSetting { get { return isSetting; } set { isSetting = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stat = GetComponent<PlayerStat>();
        backButton.onClick.AddListener(ClickBack);
        exitButton.onClick.AddListener(ClickExit);
    }


    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    
    public void SendChat()
    {
        if (input.text.Equals(""))
            return;
        string strMessage = input.text;
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

    public void OnSetting(InputValue value)
    {
            if (IsSetting == true)
            {
                settingUI.SetActive(false);
                isSetting = false;
            }
            else
            {
                settingUI.SetActive(true);
                backButton.Select();
                isSetting = true;
            }
    }

    public void ClickBack()
    {
        settingUI.SetActive(false);
    }
    public void ClickExit()
    {
        if(!stat.IsAlive)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        }
        else
            StartCoroutine(ExitRoutine());
    }

    IEnumerator ExitRoutine()
    {
        photonView.RPC("ExitDead", RpcTarget.All);
        yield return new WaitForSeconds(2f);
    }

    [PunRPC]
    public void ExitDead()
    {
        stat.IsAlive = false;
        animator.SetBool("Die", true);
    }
}
