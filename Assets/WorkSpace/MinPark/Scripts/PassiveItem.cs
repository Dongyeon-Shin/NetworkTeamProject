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
    public int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    private bool check=true;

    private void Start()
    {
        transform.parent = gameScene.transform.GetChild(0);
        boxCollider = GetComponent<BoxCollider>();
        CoeffiCient();
    }

    public void ExplosiveReact(int bombIDNumber)
    {
        Debug.Log("hit0");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("hit1");
            if (check)
            {
                gameScene.ItemDestroy(IDNumber);
                check = false;
            }
            // boxCollider.enabled=false;
            
            
        }
    }
    protected abstract void CoeffiCient();

    public void GameSceneSet(GameScene gameScene)
    {
        this.gameScene = gameScene;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
