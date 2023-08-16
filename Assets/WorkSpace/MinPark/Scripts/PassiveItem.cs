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

    private void Hit(int bombCount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameScene.ItemDestroy(IDNumber);
        }
    }

    public void ExplosiveReact(int bombCount)
    {
        Hit(bombCount);
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
