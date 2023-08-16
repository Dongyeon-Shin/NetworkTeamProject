using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] InputField input;
    [SerializeField] GameObject error;

    public void Login()
    {
        if(input.text == "")
        {
            error.SetActive(true);
            return;
        }
        PhotonNetwork.LocalPlayer.NickName = input.text;
        PhotonNetwork.ConnectUsingSettings();
    }
}
