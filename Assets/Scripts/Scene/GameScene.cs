using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            StartCoroutine(DebugGameStartRoutine());
        }
    }

    protected override IEnumerator LoadingRoutine()
    {
        if (PhotonNetwork.InRoom)
        {
            // TODO: 모두 로딩이 끝나고 카운트다운 후 시작할 것
            yield return StartCoroutine(CheckAllPlayerIsReadyRoutine());
            StartCoroutine(GameStartRoutine());
        }
        yield return null;
    }

    private IEnumerator GameStartRoutine()
    {
        yield return null;
    }

    private IEnumerator CheckAllPlayerIsReadyRoutine()
    {
        yield return null;
    }

    private IEnumerator DebugGameStartRoutine()
    {
        PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public void RequestExplosiveReaction(IExplosiveReactivable target, Bomb bomb)
    {
        photonView.RPC("SendExplosiveResult", RpcTarget.AllViaServer, target, bomb);
    }

    [PunRPC]
    private void SendExplosiveResult(IExplosiveReactivable target, Bomb bomb)
    {
        target.ExplosiveReact(bomb);
    }

}
