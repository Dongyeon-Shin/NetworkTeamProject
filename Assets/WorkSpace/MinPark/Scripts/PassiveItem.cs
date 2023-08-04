using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour, IExplosiveReactivable
{
    protected int coefficient;

    public void ExplosiveReact()
    {
        Destroy(gameObject);
    }

    protected abstract void CoeffiCient();

    private void Start()
    {
        CoeffiCient();
    }
}
