using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameScene : BaseScene
{
    [SerializeField]
    private MapData md;
    [SerializeField]
    private int totalNumberOfPlayers;
    [SerializeField]
    private int characterIndex;
    [SerializeField]
    private int countDownTime;
    private PlayerStat[] players;
    private bool[] playersReadyState;
    GameObject map;
    Transform itemArray;
    ItemSetting itemSet;
    public float LoadingProgress { get { return loadingUI.Progress; } }

    private List<IExplosiveReactivable> explosiveReactivableObjects = new List<IExplosiveReactivable>();
    private List<PassiveItem> items = new List<PassiveItem>();
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
        progress = 0f;
        loadingUI.Progress = 0.1f;
        if (PhotonNetwork.InRoom)
        {
            StartCoroutine(GameStartRoutine());
        }
        yield return null;
    }

    private IEnumerator GameStartRoutine()
    {
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        yield return StartCoroutine(MapLoadingRoutine());
        yield return StartCoroutine(PlayerLoadingRoutine());
        yield return StartCoroutine(UILoadingRoutine());
        yield return StartCoroutine(AllocateIDNumberRoutine());
        yield return StartCoroutine(WaitingForOtherPlayersRoutine());
        yield return StartCoroutine(CountDownRoutine());
    }

    private IEnumerator DebugGameStartRoutine()
    {
        loadingUI.SetLoadingMessage("Debug 모드 접속확인. 서버에 연결하는 중");
        StartCoroutine(UpdateProgressRoutine(0.2f));
        PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {UnityEngine.Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        progress = 0.5f;
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        progress = 1f;
        yield return StartCoroutine(MapLoadingRoutine());
        Debug.Log("map");
        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        //yield return new WaitWhile(() => PhotonNetwork.PlayerList.Length != totalNumberOfPlayers);
        yield return StartCoroutine(PlayerLoadingRoutine());
        Debug.Log("pla");
        yield return StartCoroutine(UILoadingRoutine());
        yield return StartCoroutine(AllocateIDNumberRoutine());
        yield return StartCoroutine(WaitingForOtherPlayersRoutine());
        yield return StartCoroutine(CountDownRoutine());
    }
    IEnumerator MapLoadingRoutine()
    {
        loadingUI.SetLoadingMessage("맵을 불러오는 중");
        StartCoroutine(UpdateProgressRoutine(0.4f));
        // 스크립터블 오브젝트 연결
        itemArray = transform.GetChild(0);
        md = GameManager.Resource.Load<MapData>("Map/MapData");
        progress = 0.1f;
        // 맵생성
        map = Instantiate(md.MapDatas[0].map);
        progress = 0.4f;
        loadingUI.SetLoadingMessage("아이템을 생성하는 중");
        itemSet = map.GetComponentInChildren<ItemSetting>();
        itemSet.ItemSettingConnect(this);
        yield return StartCoroutine(itemSet.ItemCreate());
        yield return null;
        progress = 1f;
    }

    IEnumerator PlayerLoadingRoutine()
    {
        loadingUI.SetLoadingMessage("플레이어를 생성하는 중");
        StartCoroutine(UpdateProgressRoutine(0.5f));
        players = new PlayerStat[totalNumberOfPlayers];
        playersReadyState = new bool[totalNumberOfPlayers];
        progress = 0.3f;
        GameObject player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", md.MapDatas[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
        player.GetComponent<PlayerStat>().InitialSetup(this);
        player.GetComponent<PlayerInput>().enabled = false;
        yield return null;
        progress = 1f;
    }

    IEnumerator UILoadingRoutine()
    {
        loadingUI.SetLoadingMessage("UI를 불러오는 중");
        StartCoroutine(UpdateProgressRoutine(0.6f));
        GameObject inGameInterface = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Map/GameInterFace"));
        StartCoroutine(UIWait(inGameInterface));
        yield return null;
        progress = 1f;
    }
    IEnumerator UIWait(GameObject inGameInterface)
    {
        Debug.Log(LoadingProgress);
        yield return new WaitWhile(() => players[PhotonNetwork.LocalPlayer.GetPlayerNumber()] == null);
        Debug.Log(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].InterFaceSet(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());
    }

    private IEnumerator AllocateIDNumberRoutine()
    {
        Debug.Log(LoadingProgress);
        loadingUI.SetLoadingMessage("ID Number를 부여하는 중");
        StartCoroutine(UpdateProgressRoutine(0.7f));
        // 테스트할때 빌드런, 에디터 실행을하고 얼트탭을 너무 빠르게 누르면 오류남 다시 확인해보니 그냥 그떄그때 다름
        // 그냥 지 멋대로 인듯
        yield return new WaitWhile(() => Array.Exists(players, player => player == null));
        foreach (PlayerStat player in players)
        {
            explosiveReactivableObjects.Add(player.GetComponent<IExplosiveReactivable>());
            yield return null;
        }
        progress = 0.5f;
        IExplosiveReactivable[] mapObjects = map.GetComponentsInChildren<IExplosiveReactivable>();
        explosiveReactivableObjects.AddRange(mapObjects);
        yield return new WaitWhile(() => items.Count == 0);
        Debug.Log(explosiveReactivableObjects.Count);
        explosiveReactivableObjects.AddRange(items);
        Debug.Log(explosiveReactivableObjects.Count);
        progress = 0.7f;
        for (int i = 0; i < explosiveReactivableObjects.Count; i++)
        {
            explosiveReactivableObjects[i].IDNumber = i;
            yield return null;
        }
        progress = 1f;
    }

    private IEnumerator WaitingForOtherPlayersRoutine()
    {
        loadingUI.SetLoadingMessage("다른 플레이어를 기다리는 중");
        StartCoroutine(UpdateProgressRoutine(0.9f));
        photonView.RPC("Ready", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.GetPlayerNumber());
        progress = 0.2f;
        bool waitingForOtherPlayers = true;
        while (waitingForOtherPlayers)
        {
            waitingForOtherPlayers = false;
            foreach (bool ready in playersReadyState)
            {
                if (!ready)
                {
                    waitingForOtherPlayers = true;
                }
                yield return null;
            }
            yield return null;
        }
        progress = 1f;
    }

    //TODO: waitingforotherplayerRoutine으로 타이밍이 맞는지 테스트해보고 안될시 밑의 주석처리된 코드를 사용
    private IEnumerator CountDownRoutine()
    {
        loadingUI.Progress = 1f;
        loadingUI.SetLoadingMessage("게임시작 준비 완료");
        yield return new WaitForSecondsRealtime(0.5f);
        WaitForSecondsRealtime waitASecond = new WaitForSecondsRealtime(1);
        loadingUI.FadeIn();
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = countDownTime; i > 0; i--)
        {
            Debug.Log(i);
            yield return waitASecond;
        }
        Debug.Log("GameStart!");
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerInput>().enabled = true;
    }

    //private double loadTime;
    //private IEnumerator CountDownRoutine()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        photonView.RPC("SetTimer", RpcTarget.AllViaServer, PhotonNetwork.Time);
    //    }
    //    while (countDownTime > PhotonNetwork.Time - loadTime)
    //    {
    //        int remainTime = (int)(countDownTime - (PhotonNetwork.Time - loadTime));
    //        Debug.Log($"All Player Loaded, Start count down : {remainTime + 1}");
    //        yield return new WaitForEndOfFrame();
    //    }
    //    players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerInput>().enabled = true;
    //    Debug.Log("Game Start!");
    //}

    //[PunRPC]
    //private void SetTimer(double time)
    //{
    //    loadTime = time;
    //}

    [PunRPC]
    private void Ready(int playerNumber)
    {
        playersReadyState[playerNumber] = true;
    }

    public void RegisterPlayerInfo(PlayerStat player)
    {
        players[player.PlayerNumber] = player;
    }

    // 배열 저장
    public IEnumerator ItemSetting(int[] check, int[] items) 
    {
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length == totalNumberOfPlayers);
        yield return new WaitForSeconds(1f);
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
                this.items.Add(createitem.GetComponent<PassiveItem>());
                Debug.Log(this.items.Count);
            }
        }
    }

    public void ItemDestroy(int id)
    {
        Debug.Log("item"+id);
        photonView.RPC("ItemDestroyRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void ItemDestroyRPC(int id)
    {
        for (int i = 0; i < itemArray.childCount; i++)
        {
            Debug.Log(id);
            if (itemArray.GetChild(i).GetComponent<IExplosiveReactivable>().IDNumber == id)
            {
                Debug.Log("destroy");
                Destroy(itemArray.GetChild(i).gameObject);
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
        if (chainExplosion)
        {
            bombList[explosiveReactivableObjectIndex].ExplosiveReact(bombIndex);
        }
        else
        {
            explosiveReactivableObjects[explosiveReactivableObjectIndex].ExplosiveReact(bombIndex);
        }
    }
}
