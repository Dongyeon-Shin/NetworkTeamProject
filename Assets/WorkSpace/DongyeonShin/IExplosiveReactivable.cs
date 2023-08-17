using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveReactivable
{
    int IDNumber
    {
        get;
        set;
    }
    GameScene GameScene
    {
        get;
        set;
    }
    public void ExplosiveReact(int bombID);
}
