using BaeProperty;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            loadingUI.gameObject.SetActive(false);
        }
    }

    public void GameStart()
    {
        StartCoroutine(LoadSceneRoutine(1));
    }

    protected override IEnumerator LoadingRoutine()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LocalPlayer.SetReady(false);
            yield return new WaitForSecondsRealtime(5f);
            loadingUI.Progress = 1f;
        }
        loadingUI.FadeIn();
        yield return new WaitForSecondsRealtime(0.5f);
    }
}
