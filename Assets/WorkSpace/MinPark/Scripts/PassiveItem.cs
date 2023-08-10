using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviourPun, IExplosiveReactivable
{
    protected int coefficient;
    private Bomb bomb;
    public Bomb Bomb { set { bomb=value; } }
    private int iDNumber;
    public int IDNumber { get { return iDNumber; } set { iDNumber = value; } }

    public void ExplosiveReact(Bomb bomb)
    {
        if (this.bomb == bomb)
            return;
        Destroy(gameObject);
    }
    protected abstract void CoeffiCient();

    private void Start()
    {
        CoeffiCient();
    }
}
