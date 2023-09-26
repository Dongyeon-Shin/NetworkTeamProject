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
        progress = 0f;
        loadingUI.Progress = 0.1f;
        if (!PhotonNetwork.InLobby)
        {
            loadingUI.SetLoadingMessage("로비로 돌아가는 중");
            StartCoroutine(UpdateProgressRoutine(1f));
            yield return new WaitForSecondsRealtime(1f);
            progress = 0.5f;
            PhotonNetwork.LeaveRoom();
            yield return new WaitForSecondsRealtime(1f);
            progress = 0.7f;
            PhotonNetwork.JoinLobby();
            yield return new WaitForSecondsRealtime(1f);
            progress = 0.9f;
            PhotonNetwork.LocalPlayer.SetReady(false);
            yield return new WaitForSecondsRealtime(0.1f);
            progress = 1f;
        }
        loadingUI.FadeIn();
        yield return new WaitForSecondsRealtime(0.5f);
    }
}
