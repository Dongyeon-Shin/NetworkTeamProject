using BaeProperty;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameScene : BaseScene, IPunObservable, IEventListener
{
    [SerializeField]
    private MapData md;
    GameObject map;
    [SerializeField]
    private int totalNumberOfPlayers;
    [SerializeField]
    private int countDownTime;
    private PlayerStat[] players;
    private bool[] playersReadyState;
    ItemSetting itemSet;
    HashTable mapProperty = PhotonNetwork.CurrentRoom.CustomProperties;
    int mapNumbering;
    public float LoadingProgress { get { return loadingUI.Progress; } }

    private List<IExplosiveReactivable> explosiveReactivableObjects = new List<IExplosiveReactivable>();
    private List<IExplosiveReactivable> items = new List<IExplosiveReactivable>();
    private List<Bomb> bombList = new List<Bomb>();

    private void Start()
    {
        mapNumbering = (int)mapProperty["MapNumbering"];
        totalNumberOfPlayers = PhotonNetwork.PlayerList.Length;
        if (!PhotonNetwork.InRoom)
        {
            StartCoroutine(DebugGameStartRoutine());
        }

        
    }

    private void Update()
    {
        if (IsTimer == true)
        {
            Timer();
        }
    }

    //====================== ���ӳ� ==========================
    [SerializeField] GameObject GameOverUI; // ���븮�ҽ��� ����
    private void GameOver()
    {
        if (CheckingAlive()) 
            GameOverUI.SetActive(true);
    }
    //====================== ���ӳ� ==========================

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
        loadingUI.SetLoadingMessage("Debug ��� ����Ȯ��. ������ �����ϴ� ��");
        StartCoroutine(UpdateProgressRoutine(0.2f));
        PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {UnityEngine.Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        progress = 0.5f;
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        progress = 1f;
        yield return StartCoroutine(MapLoadingRoutine());
        yield return StartCoroutine(PlayerLoadingRoutine());
        yield return StartCoroutine(UILoadingRoutine());
        yield return StartCoroutine(AllocateIDNumberRoutine());
        yield return StartCoroutine(WaitingForOtherPlayersRoutine());
        yield return StartCoroutine(CountDownRoutine());
    }
    IEnumerator MapLoadingRoutine()
    {
        loadingUI.SetLoadingMessage("���� �ҷ����� ��");
        StartCoroutine(UpdateProgressRoutine(0.4f));
        // ��ũ���ͺ� ������Ʈ ����
        md = GameManager.Resource.Load<MapData>("Map/MapData");
        progress = 0.1f;
        // �ʻ���
        map = Instantiate(md.MapDatas[mapNumbering].map);
        progress = 0.4f;
        //loadingUI.SetLoadingMessage("�������� �����ϴ� ��");
        //itemSet = map.GetComponentInChildren<ItemSetting>();
        //itemSet.ItemSettingConnect(this);
        //yield return StartCoroutine(itemSet.ItemCreate());
        yield return null;
        progress = 1f;
    }

    IEnumerator PlayerLoadingRoutine()
    {
        loadingUI.SetLoadingMessage("�÷��̾ �����ϴ� ��");
        StartCoroutine(UpdateProgressRoutine(0.5f));
        players = new PlayerStat[totalNumberOfPlayers];
        playersReadyState = new bool[totalNumberOfPlayers];
        progress = 0.3f;
        GameObject player;
        int myCount=0;
        foreach(Player roomPlayer in PhotonNetwork.PlayerList)
        {
            if(roomPlayer == PhotonNetwork.LocalPlayer)
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
        loadingUI.SetLoadingMessage("UI�� �ҷ����� ��");
        StartCoroutine(UpdateProgressRoutine(0.6f));
        GameObject inGameInterface = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Map/GameInterFace"));
        // Ÿ�̸� ���ָ� ���� ����.
        //players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerStat>().InterFaceSet(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());

        yield return null;
        progress = 1f;
    }

    private IEnumerator AllocateIDNumberRoutine()
    {
        Debug.Log(LoadingProgress);
        loadingUI.SetLoadingMessage("ID Number�� �ο��ϴ� ��");
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
        explosiveReactivableObjects.AddRange(items);
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
        loadingUI.SetLoadingMessage("�ٸ� �÷��̾ ��ٸ��� ��");
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

    //TODO: waitingforotherplayerRoutine���� Ÿ�̹��� �´��� �׽�Ʈ�غ��� �ȵɽ� ���� �ּ�ó���� �ڵ带 ���
    private IEnumerator CountDownRoutine()
    {
        loadingUI.Progress = 1f;
        loadingUI.SetLoadingMessage("���ӽ��� �غ� �Ϸ�");
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
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerInput>().enabled = true;
        GameManager.Event.AddListener(EventType.Died,this);
        TimeOut = true;
    }

    // =================================== Ÿ�̸� �� ���� üũ====================================
    [SerializeField] TMP_Text text_time;  // �ð��� ǥ���� text
    [SerializeField] float inPutTime;     // �ð�����
    private bool IsTimer = false;
    private bool TimeOut = false;

    private AudioClip backGround;


    // ���������� bgm ������ �۵�
    //private void Awake()
    //{
    //    GameManager.Sound.Init();
    //    GameManager.Sound.Play(backGround, Sound.Bgm, 1);
    //}

    private void Timer()
    {
        if(TimeOut==false)
        {
            if (inPutTime > 0)
            {
                inPutTime -= Time.deltaTime;
                text_time.text = ((int)inPutTime).ToString();
            }
            else if(inPutTime <= 0 || CheckingAlive())
            {
                text_time.text = ((int)inPutTime).ToString();
                TimeOut = true;
                IsTimer = false;
                Debug.Log("Ÿ�̸� ����");
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ��Ұ� �ΰ� �̻��϶� ������ �߿�(���� ������ �����ؾ���)
        if (stream.IsWriting)
        {
            stream.SendNext(inPutTime);
        }
        else                  // stram.IsReading
        {
            inPutTime = (float)stream.ReceiveNext();
        }
    }

    public bool CheckingAlive()
    {
        List<bool> result = new List<bool>();
        for (int i = 0; i < PhotonNetwork.CountOfPlayersInRooms; i++)
        {
            if (players[i].IsAlive == false)
            {
                result.Add(false);
                if (result.Count == PhotonNetwork.CountOfPlayersInRooms - 1 || result.Count == PhotonNetwork.CountOfPlayersInRooms)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if(eventType == EventType.Died)
        {
            CheckingAlive();
        }
    }

    // =================================== Ÿ�̸� �� ���� üũ====================================


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

    // �迭 ����
    public IEnumerator ItemSetting(int[] check, int[] items) 
    {
        yield return StartCoroutine(ItemCreate(check, items));
    }

    IEnumerator ItemCreate(int[] check, int[] items)
    {
        // ����� ���� 2���� �����ؾ� ����
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length == totalNumberOfPlayers);
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

    public void ItemDestroy(GameObject gameObject)
    {
        photonView.RPC("ItemDestroyRPC", RpcTarget.AllViaServer, gameObject);
    }

    [PunRPC]
    private void ItemDestroyRPC(GameObject gameObject)
    {
        Destroy(gameObject);
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
