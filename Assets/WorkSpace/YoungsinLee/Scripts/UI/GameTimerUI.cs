using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;

public class GameTimerUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TMP_Text text_time;// 시간을 표시할 text
    [SerializeField] float inPutTime; // 시간.
    [SerializeField] int playerCount;
    private PlayerStat[] players;
    private AudioClip backGround;
    private bool TimeOut = false;
    private int[] playerlist;

    private void Awake()
    {
        GameManager.Sound.Init();
        GameManager.Sound.Play(backGround, Sound.Bgm,1);
    }

    private void Update()
    {
        Timeree();
    }

    private void Timeree()
    {
        if (TimeOut)
        {
            return;
        }
        else
        {
            if (inPutTime > 0)
            {
                inPutTime -= Time.deltaTime;
                text_time.text = ((int)inPutTime).ToString();
            }
            else
            {
                text_time.text = ((int)inPutTime).ToString();
                TimeOut = true;
                Debug.Log("타이머 종료");
            }
        }
    }
   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
   {
       // 요소가 두개 이상일때 순서가 중요(같은 순서로 진행해야함)
       if (stream.IsWriting)
       {
           stream.SendNext(inPutTime);
       }
       else                  // stram.IsReading
       {
           inPutTime = (float)stream.ReceiveNext();
       }
   }

    public bool CheckingAlive()
    {
        for(int i =0; i < PhotonNetwork.CountOfPlayersInRooms; i++)
        {
            List<bool> result = new List<bool>();
            if (players[i].IsAlive == false)
            {
                result.Add(false);
                if(result.Count == PhotonNetwork.CountOfPlayersInRooms-1 || result.Count == PhotonNetwork.CountOfPlayersInRooms)
                {
                    return true;
                }
                else 
                    return false;
            }
        }
        return false;
    }


}

