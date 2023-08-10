using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TestUI_Chat : MonoBehaviourPunCallbacks
{
    private TMP_Text chatText;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        chatText = GameManager.Resource.Load<TMP_Text>("UI2/TextChat");
    }
}
