using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    private void OnEnable()
    {
        // TODO: �ε� �۾�
        base.OnEnable();
    }

    protected abstract IEnumerator LoadingRoutine();
}
