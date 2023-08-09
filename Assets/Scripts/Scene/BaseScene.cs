using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    private void OnEnable()
    {
        // TODO: 로딩 작업
        
    }

    protected abstract IEnumerator LoadingRoutine();
}
