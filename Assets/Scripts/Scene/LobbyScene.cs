using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
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
