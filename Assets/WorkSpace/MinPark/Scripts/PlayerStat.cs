using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviourPunCallbacks
{
    public TMP_Text[] texts = new TMP_Text[3];
    public TMP_Text power_Text;
    public TMP_Text speed_Text;
    public TMP_Text bomb_Text;
    private bool isAlive = true;
    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    private GameScene gameScene;
    public GameScene GameScene { get { return gameScene; } set { gameScene = value; } }
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } }
    [SerializeField]
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }


    private void Start()
    {
        StartCoroutine(AllocatePlayerNumberRoutine());
    }
    public void InterFaceSet(TMP_Text[] tmp)
    {
        texts = tmp;
        power_Text = texts[0];
        speed_Text = texts[1];
        bomb_Text = texts[2];
    }

    [SerializeField]
    private int power = 1;
    [SerializeField]
    private int bomb = 1;
    [SerializeField]
    private int speed = 1;

    public int Power { get { return power; } set {  power += value; } }
    public int Bomb { get {  return bomb; } set {  bomb += value; } }
    public int Speed {  get { return speed; } set {  speed += value; } }

    public void ItemInterfaceSet()
    {
        photonView.RPC("ItemInterFaceSetRPC", RpcTarget.AllViaServer);
    }
    [PunRPC]
    private void ItemInterFaceSetRPC()
    {
        if (photonView.IsMine)
        {
            power_Text.text = $"{power - 1}";
            speed_Text.text = $"{speed - 1}";
            bomb_Text.text = $"{bomb - 1}";
        }
    }

    public void StatRenewal()
    {
        photonView.RPC("StatRPC", RpcTarget.AllViaServer, power, bomb, speed);
    }
    [PunRPC]
    private void StatRPC(int power, int bomb, int speed)
    {
        this.power = power;
        this.bomb = bomb;
        this.speed = speed;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        StartCoroutine(AllocatePlayerNumberRoutine());
    }  

    private IEnumerator AllocatePlayerNumberRoutine()
    {
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        playerNumber = photonView.ViewID / 1000 - 1;
        if (gameScene == null)
        {
            gameScene = GameObject.FindObjectOfType<GameScene>();
        }
        yield return new WaitWhile(() => gameScene.LoadingProgress < 0.3f);
        gameScene.RegisterPlayerInfo(this);
        isAlive = true;
    }
    public void InitialSetup(GameScene gameScene)
    {
        this.gameScene = gameScene;
        isAlive = true;
    }

    public void OnPreNetDestroy(PhotonView rootView)
    {
        // 시체생성
    }
}
