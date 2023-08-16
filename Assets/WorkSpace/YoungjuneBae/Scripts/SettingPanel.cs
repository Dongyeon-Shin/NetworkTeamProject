using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    public void LogOut()
    {
        PhotonNetwork.Disconnect();
    }
}
