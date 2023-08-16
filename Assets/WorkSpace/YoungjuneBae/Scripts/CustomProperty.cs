using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTabel = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public static bool GetReady(this Player player)
    {
        HashTabel property = player.CustomProperties;
        if (property.ContainsKey("Ready"))
        {
            return (bool)property["Ready"];
        }
        else
        { return false; }
    }
    public static void SetReady(this Player player, bool ready)
    {
        HashTabel property = player.CustomProperties;
        property["Ready"] = ready;
        player.SetCustomProperties(property);
    }
    public static bool GetLoad(this Player player)
    {

        HashTabel property = player.CustomProperties;
        if (property.ContainsKey("Load"))
        {
            return (bool)property["Load"];
        }
        else
        { return false; }
    }


    public static void SetLoad(this Player player, bool ready)
    {
        HashTabel property = player.CustomProperties;
        property["Load"] = ready;
        player.SetCustomProperties(property);
    }

    public static int GetLoadTime(this Room room)
    {
        HashTabel property = room.CustomProperties;
        if (property.ContainsKey("LoadTime"))
        {
            return (int)property["LoadTime"];
        }
        else
        { return -1; }
    }

    public static void SetLoadTime(this Room room, int loadTime)
    {
        HashTabel property = room.CustomProperties;
        property["LoadTime"] = loadTime;
        room.SetCustomProperties(property);
    }
}
