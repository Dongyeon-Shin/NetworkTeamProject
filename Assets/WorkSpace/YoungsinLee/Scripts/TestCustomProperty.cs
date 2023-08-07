using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class TestCustomProperty
{
    public static bool GetLoad(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey("Load"))
        {
            return (bool)property["Load"];
        }
        else
            return false;
    }

    public static void SetLoad(this Player player, bool load)
    {
        PhotonHashtable property = player.CustomProperties;
        property["Load"] = load;
        player.SetCustomProperties(property);
    }

    public static int GetLoadTime(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey("LoadTime"))
        {
            return (int)property["LoadTime"];
        }
        else
            return -1;
    }

    public static void SetLoadTime(this Room room, int loadTime)
    {
        PhotonHashtable property = room.CustomProperties;
        property["LoadTime"] = loadTime;
        room.SetCustomProperties(property);
    }
}
