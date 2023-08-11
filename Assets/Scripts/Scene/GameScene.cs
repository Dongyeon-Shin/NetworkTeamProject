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
    GameObject map;
    public bool itemSetTrue=false;
    int[] check;
    int[] items;

    private List<IExplosiveReactivable> explosiveReactivableObjects = new List<IExplosiveReactivable>();
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
        StartPointData startPointData = GameManager.Resource.Load<StartPointData>("Map/StartPointData");
        map = Instantiate(st.StartPoints[0].map);
        itemSet = map.GetComponentInChildren<ItemSetting>();
        itemSet.ItemSettingConnect(this);
        itemSetting = itemSet.transform;
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        PhotonNetwork.Instantiate("Prefab/Player_ver0.1/Player_Reindeer", startPointData.StartPoints[0].position[PhotonNetwork.LocalPlayer.GetPlayerNumber()], Quaternion.Euler(0, 0, 0)).GetComponent<PlayerStat>().InitialSetup(this);
    }

    // �迭 ����
    public void ArrayCopy(int[] check, int[] item) 
    {
        photonView.RPC("ArrayCopyRPC", RpcTarget.AllViaServer, check, item);
    }

    [PunRPC]
    private void ArrayCopyRPC(int[] check, int[] item)
    {
        this.check = check;
        this.items = item;
    }

    public void ItemCreate()
    {
        StartCoroutine(ItemCreateDelay());
    }
    IEnumerator ItemCreateDelay()
    {
        yield return new WaitWhile(() => this.check == null);
        for (int i = 0; i < itemSetting.childCount; i++)
        {
            if (this.check[i] == 1)
            {
                GameObject createitem = itemSetting.GetChild(i).GetComponent<Box>().item = itemSet.itemArray[items[i]];
                createitem.GetComponent<PassiveItem>();
                IExplosiveReactivable item = createitem.GetComponent<IExplosiveReactivable>();
                item.IDNumber = explosiveReactivableObjects.Count;
                explosiveReactivableObjects.Add(item);
            }
        }
        RegisterMapObjectsID(map);
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
            Debug.Log("C");
            explosiveReactivableObjects.Add(mapObjects[i - itemindex]);
            Debug.Log("C2");
        }
    }

    public void RegisterPlayerID(GameObject player)
    {
        //TODO: Test �ʿ� �÷��̾ ���ӵǴ� ������� playernumbering�� �ΰ��Ǵϱ�
        // �ش� �Լ��� playernumber�� ���� ������ ȣ��ȴٴ� ����
        // ������ ������ local���� �ٸ� �÷��̾� ������Ʈ�� PhotonNetwork instantiate�̱� ������
        // Ȥ�� �𸣴� Ȯ���Ұ�
        explosiveReactivableObjects.Add(player.GetComponent<IExplosiveReactivable>());
        Debug.Log(player.GetComponent<PlayerStat>().PlayerNumber);
    }

    public void RegisterBombID(Bomb bomb)
    {
        bomb.IDNumber = bombList.Count;
        bombList.Add(bomb);
    }

    public void RequestExplosiveReaction(IExplosiveReactivable target, int bombIndex)
    {
        photonView.RPC("SendExplosionResult", RpcTarget.AllViaServer, target.IDNumber, bombIndex);
    }

    [PunRPC]
    private void SendExplosionResult(int explosiveReactivableObjectIndex, int bombIndex)
    {
        explosiveReactivableObjects[explosiveReactivableObjectIndex].ExplosiveReact(bombList[bombIndex]);
    }

}
