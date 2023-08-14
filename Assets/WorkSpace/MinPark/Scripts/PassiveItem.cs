using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour, IExplosiveReactivable
{
    protected int coefficient;
    public GameObject itemArray;
    private Bomb bomb;
    public Bomb Bomb { set { bomb=value; } }
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

    public void ExplosiveReact(Bomb bomb)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (this.bomb == bomb)
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
