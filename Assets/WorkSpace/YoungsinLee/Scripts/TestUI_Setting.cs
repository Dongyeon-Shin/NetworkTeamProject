using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI_Setting : MonoBehaviourPun
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
