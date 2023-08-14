using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    protected abstract IEnumerator LoadingRoutine();
}
