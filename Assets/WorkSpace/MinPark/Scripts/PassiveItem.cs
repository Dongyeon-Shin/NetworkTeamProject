using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour, IExplosiveReactivable
{
    protected int coefficient;
    public GameObject itemArray;
    private int bombIDNumber;
    public int BombIDNumber { set { bombIDNumber=value; } }
    public GameScene gameScene;
    private BoxCollider boxCollider;
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    private void Start()
    {
        transform.parent = gameScene.transform.GetChild(0);
        boxCollider = GetComponent<BoxCollider>();
        CoeffiCient();
    }

    public void ExplosiveReact(int bombIDNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (this.bombIDNumber == bombIDNumber)
                return;
            boxCollider.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    protected abstract void CoeffiCient();

    public void GameSceneSet(GameScene gameScene)
    {
        this.gameScene = gameScene;
    }
}
