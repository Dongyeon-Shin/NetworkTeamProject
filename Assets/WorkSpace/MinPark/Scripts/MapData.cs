using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "MapData",menuName = "Data/mapData")]
public class MapData : ScriptableObject
{
    [SerializeField] MapDataInfo[] mapData;

    public MapDataInfo[] MapDatas { get { return mapData; } }

    [Serializable]
    public class MapDataInfo
    {
        public int maxPlayer;
        public Vector3[] position;
        public GameObject map;
    }
}
