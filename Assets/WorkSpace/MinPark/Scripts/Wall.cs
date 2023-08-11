using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IExplosiveReactivable
{
    [SerializeField]
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }
    public void ExplosiveReact(Bomb bomb)
    {
        
    }
}
