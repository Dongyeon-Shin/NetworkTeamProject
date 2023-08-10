using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    private StartPointData st;
    Transform itemSetting;
    ItemSetting itemSet;

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
        StartPointData startPointData = GameManager.Resource.Load<StartPointData>("Map/StartPointData");
        itemSet = Instantiate(st.StartPoints[0].map).GetComponentInChildren<ItemSetting>();
        itemSet.ItemSettingConnect(this);
        itemSetting = itemSet.transform;
        Debug.Log(startPointData);
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        //PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", startPointData.StartPoints[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0)).GetComponent<PlayerStat>().InitialSetup(this);
        PhotonNetwork.InstantiateRoomObject("Prefab/Player_ver0.1/Player_Reindeer", startPointData.StartPoints[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.identity);
        //yield return new WaitWhile(() => player == null);
        //player.GetComponent<PlayerStat>().InitialSetup(this);
        //yield return new WaitUntil(() => PhotonNetwork.InRoom);
    }

    public void ItemCreate(int[] check, int[] item)
    {
        photonView.RPC("ItemInsult", RpcTarget.AllViaServer, check, item);
        
    }
    [PunRPC]
    private void ItemInsult(int[] check, int[] item)
    {
        Debug.Log("2");
        for (int i = 0; i < itemSetting.childCount; i++)
        {
            if (check[i] == 1)
            {
                itemSetting.GetChild(i).GetComponent<Box>().item = itemSet.itemArray[item[i]];
            }
        }
    }
    

    private void Test()
    {
        photonView.RPC("TestRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void TestRPC()
    {
        Debug.Log("Check");
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
