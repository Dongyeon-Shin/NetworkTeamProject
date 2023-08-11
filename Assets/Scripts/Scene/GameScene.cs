using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    private MapData md;

    GameObject map;
    [SerializeField]
    int playerCount;
    GameObject[] players;
    // ���� ���̵�� ���� �ȳ��� �ϴ� �̴����
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
            // TODO: ��� �ε��� ������ ī��Ʈ�ٿ� �� ������ ��
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
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
    }
    IEnumerator MapLoadingRoutine()
    {
        // ��ũ���ͺ� ������Ʈ ����
        md = GameManager.Resource.Load<MapData>("Map/MapData");
        // �ʻ���
        map = Instantiate(md.MapDatas[0].map);
        itemSet = map.GetComponentInChildren<ItemSetting>();
        itemSet.ItemSettingConnect(this);
        itemSet.ItemCreate();
        yield return null;
    }

    IEnumerator PlayerLoadingRoutine()
    {
        players = new GameObject[playerCount];
        GameObject player = PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", md.MapDatas[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0));
        player.GetComponent<PlayerStat>().InitialSetup(this);
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()] = player;
        yield return null;
    }

    IEnumerator UILoadingRoutine()
    {
        GameObject inGameInterface = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Map/GameInterFace"));
        // Ÿ�̸� ���ָ� ���� ����.
        players[PhotonNetwork.LocalPlayer.GetPlayerNumber()].GetComponent<PlayerStat>().InterFaceSet(inGameInterface.transform.GetChild(2).GetComponentsInChildren<TMP_Text>());

        yield return null;
    }
    private IEnumerator AllocateIDNumberRoutine()
    {
        yield return null;
    }



    // �迭 ����
    public void ItemSetting(int[] check, int[] items) 
    {
        StartCoroutine(ItemCreate());

        IEnumerator ItemCreate()
        {
            // ����� ���� 2���� �����ؾ� ����
            yield return new WaitWhile(() => PhotonNetwork.PlayerList.Length != 2);
            photonView.RPC("ItemCreateRPC", RpcTarget.AllViaServer, check, items);
        }
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

    private void RegisterMapObjectsID(GameObject map)
    {
        IExplosiveReactivable[] mapObjects = map.GetComponentsInChildren<IExplosiveReactivable>();
        int itemindex= explosiveReactivableObjects.Count;
        for (int i = itemindex; i < itemindex + mapObjects.Length;i++)
        {
            mapObjects[i-itemindex].IDNumber = i;
            explosiveReactivableObjects.Add(mapObjects[i - itemindex]);
        }
    }

    public void RegisterPlayerID(GameObject player, ref int iDNumber)
    {
        //TODO: Test �ʿ� �÷��̾ ���ӵǴ� ������� playernumbering�� �ΰ��Ǵϱ�
        // �ش� �Լ��� playernumber�� ���� ������ ȣ��ȴٴ� ����
        // ������ ������ local���� �ٸ� �÷��̾� ������Ʈ�� PhotonNetwork instantiate�̱� ������
        // Ȥ�� �𸣴� Ȯ���Ұ�
        iDNumber = explosiveReactivableObjects.Count;
        explosiveReactivableObjects.Add(player.GetComponent<IExplosiveReactivable>());
        Debug.Log(player.GetComponent<PlayerStat>().PlayerNumber);
    }

    public void RegisterBombID(Bomb bomb)
    {
        bomb.IDNumber = bombList.Count;
        bombList.Add(bomb);
    }

    public void RequestExplosiveReaction(IExplosiveReactivable target, int bombIndex, bool chainExplosion)
    {
        Debug.Log(target.IDNumber);
        photonView.RPC("SendExplosionResult", RpcTarget.AllViaServer, target.IDNumber, bombIndex, chainExplosion);
    }

    [PunRPC]
    private void SendExplosionResult(int explosiveReactivableObjectIndex, int bombIndex, bool chainExplosion)
    {
        if (chainExplosion)
        {
            bombList[explosiveReactivableObjectIndex].ExplosiveReact(bombList[bombIndex]);
        }
        else
        {
            explosiveReactivableObjects[explosiveReactivableObjectIndex].ExplosiveReact(bombList[bombIndex]);
        }
    }

}
