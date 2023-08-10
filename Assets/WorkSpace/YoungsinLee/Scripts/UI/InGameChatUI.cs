using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameChatUI : MonoBehaviourPun
{
    private Camera cameraToLookAt;

    private void Awake()
    {
        cameraToLookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cameraToLookAt.transform.forward);
    }
}
