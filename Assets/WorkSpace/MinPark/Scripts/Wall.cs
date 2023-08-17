using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IExplosiveReactivable
{
    [SerializeField]
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }
    private GameScene gameScene;
    public GameScene GameScene { get { return gameScene; } set {  gameScene = value; } }
    public void ExplosiveReact(int bombIDNumber)
    {
        gameScene.ExplodeABomb(bombIDNumber);
    }
}
