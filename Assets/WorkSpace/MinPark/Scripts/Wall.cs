using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IExplosiveReactivable
{
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }
    public void ExplosiveReact(Bomb bomb)
    {

    }
}
