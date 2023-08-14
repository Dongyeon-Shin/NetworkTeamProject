using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    private MapData md;
    [SerializeField]
    int playerCount;

    GameObject map;
    GameObject[] players;
    Transform itemArray;
    // 아직 아이디어 생각 안나서 일단 이대로함
    ItemSetting itemSet;

    private List<IExplosiveReactivable> explosiveReactivableObjects = new List<IExplosiveReactivable>();
    private List<IExplosiveReactivable> items = new List<IExplosiveReactivable>();
    private List<Bomb> bombList = new List<Bomb>();

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
            yield return StartCoroutine(WaitingForOtherPlayersRoutine());
            StartCoroutine(GameStartRoutine());
        }
        yield return null;
    }

    private IEnumerator GameStartRoutine()
    {
        yield return null;
    }

    private IEnumerator DebugGameStartRoutine()
    {
        PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        //Time.timeScale = 0f;
        yield return StartCoroutine(MapLoadingRoutine());
        yield return StartCoroutine(PlayerLoadingRoutine());
        yield return StartCoroutine(UILoadingRoutine());
        yield return StartCoroutine(AllocateIDNumberRoutine());
        yield return StartCoroutine(WaitingForOtherPlayersRoutine());
        Time.timeScale = 1f;
    }
    IEnumerator MapLoadingRoutine()
    {
        // 스크립터블 오브젝트 연결
        md = GameManager.Resource.Load<MapData>("Map/MapData");
        // 맵생성
        map = Instantiate(md.MapDatas[0].map);
        itemArray = transform.GetChild(0);
        itemSet = map.GetComponentInChildren<ItemSetting>();
        itemSet.ItemSettingConnect(this);
        itemSet.ItemCreate();
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerLoadingRoutine()
    {
        players = new GameObject[playerCount];
        GameObject player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", md.MapDatas[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        playerStat.InitialSetup(this);
        //players[playerStat.PlayerNumber] = player;
        yield return null;
    }

    IEnumerator UILoadingRoutine()
    {
        GameObject inGameInterface = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Map/GameInterFace"));
        // 타이머 없애면 쉽게 가능.
        //players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerStat>().InterFaceSet(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());

        yield return null;
    }

    private IEnumerator AllocateIDNumberRoutine()
    {
        foreach (GameObject player in players)
        {
            explosiveReactivableObjects.Add(player.GetComponent<IExplosiveReactivable>());
        }
        IExplosiveReactivable[] mapObjects = map.GetComponentsInChildren<IExplosiveReactivable>();
        explosiveReactivableObjects.AddRange(mapObjects);
        explosiveReactivableObjects.AddRange(items);
        for (int i = 0; i < explosiveReactivableObjects.Count; i++)
        {
            explosiveReactivableObjects[i].IDNumber = i;
        }
        yield return null;
    }

    private IEnumerator WaitingForOtherPlayersRoutine()
    {
        yield return null;
    }

    public void ItemSettingStart(int[] check, int[] items)
    {
        StartCoroutine(ItemCreate(check, items));
    }

    IEnumerator ItemCreate(int[] check, int[] items)
    {
        // 디버그 모드시 2명이 접속해야 실행
        yield return new WaitWhile(() => PhotonNetwork.PlayerList.Length != 2);
        photonView.RPC("ItemCreateRPC", RpcTarget.AllViaServer, check, items);
    }

    [PunRPC]
    private void ItemCreateRPC(int[] check, int[] items)
    {
        for (int i = 0; i < check.Length; i++)
        {
            if (check[i] == 1)
            {
                GameObject createitem = itemSet.transform.GetChild(i).GetComponent<Box>().item = itemSet.itemArray[items[i]];
                createitem.GetComponent<PassiveItem>().GameSceneSet(this);
                IExplosiveReactivable item = createitem.GetComponent<IExplosiveReactivable>();
                this.items.Add(item);
            }
        }
    }

    public void ItemDestroy(int id)
    {
        photonView.RPC("ItemDestroyRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void ItemDestroyRPC(int id)
    {
        for (int i = 0; i < itemArray.childCount; i++)
        {
            if (itemArray.GetChild(i).GetComponent<IExplosiveReactivable>().IDNumber == id)
            {
                Destroy(itemArray.GetChild(i));
                break;
            }
        }
    }


    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public void RegisterBombID(Bomb bomb)
    {
        bomb.IDNumber = bombList.Count;
        bombList.Add(bomb);
    }

    public void RequestExplosiveReaction(IExplosiveReactivable target, int bombIndex, bool chainExplosion)
    {
        photonView.RPC("SendExplosionResult", RpcTarget.AllViaServer, target.IDNumber, bombIndex, chainExplosion);
    }

    [PunRPC]
    private void SendExplosionResult(int explosiveReactivableObjectIndex, int bombIndex, bool chainExplosion)
    {
        Debug.Log(explosiveReactivableObjectIndex);
        if (chainExplosion)
        {
            bombList[explosiveReactivableObjectIndex].ExplosiveReact(bombList[bombIndex]);
        }
        else
        {
            Debug.Log("hit");
            explosiveReactivableObjects[explosiveReactivableObjectIndex].ExplosiveReact(bombList[bombIndex]);
        }
    }

}
