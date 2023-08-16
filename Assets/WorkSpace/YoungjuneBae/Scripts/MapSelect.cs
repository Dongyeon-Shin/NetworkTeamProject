using Microsoft.Unity.VisualStudio.Editor;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class MapSelect : MonoBehaviour
{
    [SerializeField] GameObject[] map;
    [SerializeField] Button PlayButton;
    [SerializeField] Text mapText;

    int mapCount = 0;

    private void Awake()
    {
        map[mapCount].SetActive(true);
        mapText.text = map[mapCount].name;

    }
    public void LeftButton()
    {
        if(mapCount==0)
        {
            map[mapCount].gameObject.SetActive(false);
            mapCount = map.Length-1;
            map[mapCount].gameObject.SetActive(true);
            mapText.text = map[mapCount].name;
        }
        else
        {
            map[mapCount].gameObject.SetActive(false);
            mapCount--;
            map[mapCount].gameObject.SetActive(true);
            mapText.text = map[mapCount].name;
        }
    }

    public void RightButton()
    {
        if (mapCount == map.Length - 1)
        {
            map[mapCount].gameObject.SetActive(false);
            mapCount = 0;
            map[mapCount].gameObject.SetActive(true);
            mapText.text = map[mapCount].name;

        }
        else
        {
            map[mapCount].gameObject.SetActive(false);
            mapCount++;
            map[mapCount].gameObject.SetActive(true);
            mapText.text = map[mapCount].name;
        }
    }

    public void GameStart()
    {
        HashTable MapNumber = PhotonNetwork.CurrentRoom.CustomProperties;
        MapNumber["MapNumbering"] = mapCount;
    }
}
