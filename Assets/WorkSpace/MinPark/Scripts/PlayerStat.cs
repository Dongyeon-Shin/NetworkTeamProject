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
    public GameScene GameScene {  get { return gameScene; } }
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } }

    private void Start()
    {
        //texts = GameScene.gameInterFace.GetComponentsInChildren<TMP_Text>();
        power_Text = texts[0];
        speed_Text = texts[1];
        bomb_Text = texts[2];
        StartCoroutine(AllocatePlayerNumberRoutine());
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
        //power_Text.text = $"{power-1}";
        //speed_Text.text = $"{speed - 1}";
        //bomb_Text.text = $"{bomb - 1}";
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        StartCoroutine(AllocatePlayerNumberRoutine());
    }  

    private IEnumerator AllocatePlayerNumberRoutine()
    {
        yield return new WaitWhile(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() == -1);
        playerNumber =  PhotonNetwork.LocalPlayer.GetPlayerNumber();
        gameScene.RegisterPlayerID(gameObject);
    }
    public void InitialSetup(GameScene gameScene)
    {
        this.gameScene = gameScene;
        isAlive = true;
    }
}
