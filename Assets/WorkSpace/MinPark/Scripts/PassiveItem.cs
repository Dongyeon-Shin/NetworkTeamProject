using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour
{
    protected int coefficient;
    protected abstract void CoeffiCient();

    private void Start()
    {
        CoeffiCient();
    }
}
