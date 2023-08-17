using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour, IExplosiveReactivable
{
    protected int coefficient;
    public GameObject itemArray;
    public GameScene gameScene;
    public GameScene GameScene { get { return gameScene; } set {  gameScene = value; } }
    private BoxCollider boxCollider;
    public int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }
    private int bombNumber;

    private void Start()
    {
        transform.parent = gameScene.transform.GetChild(1);
        boxCollider = GetComponent<BoxCollider>();
        CoeffiCient();
    }

    private void Hit(int bombIDNumber)
    {
        gameScene.ExplodeABomb(bombIDNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            gameScene.ItemDestroy(IDNumber);
        }
    }

    public void ExplosiveReact(int bombIDNumber)
    {
        gameScene.ExplodeABomb(bombIDNumber);
        Hit(bombIDNumber);
    }
    protected abstract void CoeffiCient();

    public void GameSceneSet(GameScene gameScene)
    {
        this.gameScene = gameScene;
    }

    public void SetBombNumber(int bombNumber)
    {
        this.bombNumber = bombNumber;
    }
}
