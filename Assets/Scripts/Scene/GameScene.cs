using BaeProperty;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameScene : BaseScene, IPunObservable, IEventListener
{
    [SerializeField]
    private MapData md;
    [SerializeField]
    private int totalNumberOfPlayers;
    [SerializeField]
    private int countDownTime;
    private PlayerStat[] players;
    private bool[] playersReadyState;
    GameObject map;
    Transform itemArray;
    ItemSetting itemSet;
    int bombCount;

    public float LoadingProgress { get { return loadingUI.Progress; } }

    private List<IExplosiveReactivable> explosiveReactivableObjects = new List<IExplosiveReactivable>();
    private List<PassiveItem> items = new List<PassiveItem>();
    private List<Bomb> bombList = new List<Bomb>();
    int mapNumbering;
    private bool isGameOver = false;
    private void Start()
    {
        HashTable mapProperty = PhotonNetwork.CurrentRoom.CustomProperties;
        mapNumbering = (int)mapProperty["MapNumbering"];
        totalNumberOfPlayers = PhotonNetwork.PlayerList.Length;
        photonView.RPC("SyncGameStart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void SyncGameStart()
    {
        if (!PhotonNetwork.InRoom)
        {
            StartCoroutine(LoadingRoutine());
        }
    }

    private void Update()
    {
        if (IsTimer == true)
        {
            Timer();
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

    public void ExitGame()
    {
        Debug.Log(true);
        Application.Quit();
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
        GameManager.Event.AddListener(EventType.Died, this);
    }

   //private IEnumerator DebugGameStartRoutine()
   //{
   //    loadingUI.SetLoadingMessage("Debug 모드 접속확인. 서버에 연결하는 중");
   //    StartCoroutine(UpdateProgressRoutine(0.2f));
   //    PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {UnityEngine.Random.Range(1000, 10000)}";
   //    PhotonNetwork.ConnectUsingSettings();
   //    yield return new WaitUntil(() => PhotonNetwork.InRoom);
   //    progress = 0.5f;
   //    yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
   //    progress = 1f;
   //    yield return StartCoroutine(MapLoadingRoutine());
   //    Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
   //    //yield return new WaitWhile(() => PhotonNetwork.PlayerList.Length != totalNumberOfPlayers);
   //    yield return StartCoroutine(PlayerLoadingRoutine());
   //    yield return StartCoroutine(UILoadingRoutine());
   //    yield return StartCoroutine(AllocateIDNumberRoutine());
   //    yield return StartCoroutine(WaitingForOtherPlayersRoutine());
   //    yield return StartCoroutine(CountDownRoutine());
   //}
    IEnumerator MapLoadingRoutine()
    {
        loadingUI.SetLoadingMessage("맵을 불러오는 중");
        StartCoroutine(UpdateProgressRoutine(0.4f));
        // 스크립터블 오브젝트 연결
        itemArray = transform.GetChild(1);
        md = GameManager.Resource.Load<MapData>("Map/MapData");
        progress = 0.1f;
        // 맵생성
        map = Instantiate(md.MapDatas[mapNumbering].map);
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
        GameObject player;
        int myCount = 0;
        foreach (Player roomPlayer in PhotonNetwork.PlayerList)
        {
            if (roomPlayer == PhotonNetwork.LocalPlayer)
            {
                break;
            }
            else
            {
                myCount++;
            }
        }

        switch (PhotonNetwork.LocalPlayer.GetColor())
        {
            case 0:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", md.MapDatas[mapNumbering].position[myCount], Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Santa", md.MapDatas[mapNumbering].position[myCount], Quaternion.Euler(0, 0, 0));
                break;
            case 2:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Snow_Princess", md.MapDatas[mapNumbering].position[myCount], Quaternion.Euler(0, 0, 0));
                break;
            default:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Snowmon", md.MapDatas[mapNumbering].position[myCount], Quaternion.Euler(0, 0, 0));
                break;
        }/*
        switch (PhotonNetwork.LocalPlayer.GetPlayerNumber() % 4)
        {
            case 0:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", md.MapDatas[mapNumbering].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Santa", md.MapDatas[mapNumbering].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
                break;
            case 2:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Snow_Princess", md.MapDatas[mapNumbering].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
                break;
            default:
                player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Snowmon", md.MapDatas[mapNumbering].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
                break;
        }*/
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
        yield return new WaitWhile(() => players[PhotonNetwork.LocalPlayer.GetPlayerNumber()] == null);
        Debug.Log(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].InterFaceSet(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());
    }

    private IEnumerator AllocateIDNumberRoutine()
    {
        Debug.Log(LoadingProgress);
        loadingUI.SetLoadingMessage("ID Number를 부여하는 중");
        StartCoroutine(UpdateProgressRoutine(0.7f));
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
        explosiveReactivableObjects.AddRange(items);
        Debug.Log(explosiveReactivableObjects.Count);
        progress = 0.7f;
        for (int i = 0; i < explosiveReactivableObjects.Count; i++)
        {
            if (explosiveReactivableObjects[i].GameScene == null)
            {
                explosiveReactivableObjects[i].GameScene = this;
            }
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
        GameOverUI.SetActive(false);
        loadingUI.Progress = 1f;
        loadingUI.SetLoadingMessage("게임시작 준비 완료");
        yield return new WaitForSecondsRealtime(0.5f);
        WaitForSecondsRealtime waitASecond = new WaitForSecondsRealtime(1);
        loadingUI.FadeIn();
        yield return new WaitForSecondsRealtime(0.5f);
        countDownNumber.gameObject.SetActive(true);
        for (int i = countDownTime; i > 0; i--)
        {
            countDownNumber.mesh = numbers[i];
            yield return waitASecond;
        }
        countDownNumber.gameObject.SetActive(false);

        GameManager.Sound.Init();
        GameManager.Sound.Play("Sounds/BGM/BackBGM_2", Sound.Bgm);
        IsTimer = true;
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerInput>().enabled = true;
    }

    // =================================== 타이머 및 생존 체크====================================
    [SerializeField] TMP_Text text_time;  // 시간을 표시할 text
    private float inPutTime = 300;     // 시간설정
    private bool IsTimer = false;
    private bool TimeOut = false;

    private void Timer()
    {
        if (TimeOut == false)
        {
            if (inPutTime > 0)
            {
                inPutTime -= Time.deltaTime;
                text_time.text = ((int)inPutTime).ToString();
            }
            else if (inPutTime <= 0 || isGameOver == true)
            {
                text_time.text = ((int)inPutTime).ToString();
                TimeOut = true;
                IsTimer = false;
                Debug.Log("타이머 종료");
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 요소가 두개 이상일때 순서가 중요(같은 순서로 진행해야함)
        if (stream.IsWriting)
        {
            stream.SendNext(inPutTime);
        }
        else                  // stram.IsReading
        {
            inPutTime = (float)stream.ReceiveNext();
        }
    }
    List<int> result = new List<int>();
    public void CheckingAlive()
    {
        for (int i = 0; i < PhotonNetwork.CountOfPlayersInRooms; i++)
        {
            if (players[i].IsAlive == false)
            {
                Debug.Log("큰응애");
                result.Add(i);
                if (result.Count - 1 > PhotonNetwork.CountOfPlayersInRooms - 1 || result.Count - 1 == PhotonNetwork.CountOfPlayersInRooms)
                {
                    Debug.Log("응애");
                    isGameOver = true;
                }
            }
            else
                Debug.Log("낫응애");
            i++;
        }

        if (isGameOver == true)
            photonView.RPC("GameOver", RpcTarget.AllViaServer);
    }
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if (eventType == EventType.Died)
        {
            CheckingAlive();
        }
    }

    private IEnumerator InGameRoutine()
    {
        bool isGameRunning = true;
        while (isGameRunning)
        {
            yield return null;
            int count = totalNumberOfPlayers;
            foreach (PlayerStat player in players)
            {
                if (!player.IsAlive)
                {
                    count--;
                }
                yield return null;
            }
            if (count == 1)
            {
                isGameRunning = false;
            }
        }
        Debug.Log("GameFinish");
        photonView.RPC("GameOver", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void GameOver()
    {
        GameOverUI.SetActive(true);
        //StartCoroutine(QuitRoutine());
        StartCoroutine(LoadSceneRoutine(0));
    }

    //private IEnumerator QuitRoutine()
    //{
    //    yield return new WaitForSecondsRealtime(5f);
    //    Application.Quit();
    //}
    //
    //====================== 게임끝 ==========================
    [SerializeField] GameObject GameOverUI; // 공용리소스에 있음
    //====================== 게임끝 ==========================

    // =================================== 타이머 및 생존 체크====================================


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

    public void RequestExplosiveReaction(IExplosiveReactivable target, int bombIDNumber, bool chainExplosion)
    {
        photonView.RPC("SendExplosionResult", RpcTarget.AllViaServer, target.IDNumber, bombIDNumber, chainExplosion);
    }

    [PunRPC]
    private void SendExplosionResult(int explosiveReactivableObjectIndex, int bombIDNumber, bool chainExplosion)
    {
        if (chainExplosion)
        {
            bombList[explosiveReactivableObjectIndex].ExplosiveReact(bombIDNumber);
        }
        else
        {
            explosiveReactivableObjects[explosiveReactivableObjectIndex].ExplosiveReact(bombIDNumber);
        }
    }

    public void ExplodeABomb(int bombIDNumber)
    {
        Debug.Log(bombIDNumber);
        bombList[bombIDNumber].BombState = true;
    }

    public void CountBomb()
    {
        bombCount++;
    }
}
