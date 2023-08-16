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
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(LoadSceneRoutine(1));
    }

    protected override IEnumerator LoadingRoutine()
    {
        if (!PhotonNetwork.InLobby)
        {
            
        }
        yield return null;
    }

    private void JoinLobby()
    {

    }
}
