using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "StartPointData",menuName = "Data/StartPoint")]
public class StartPointData : ScriptableObject
{
    [SerializeField] StartPointInfo[] startPoints;

    public StartPointInfo[] StartPoints { get { return startPoints; } }

    [Serializable]
    public class StartPointInfo
    {
        public int maxPlayer;
        public Vector3[] position;
        public GameObject map;
    }
}
