using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;

public class GameTimerUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TMP_Text text_time;// �ð��� ǥ���� text
    [SerializeField] float inPutTime; // �ð�.
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
                Debug.Log("Ÿ�̸� ����");
            }
        }
    }
   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
   {
       // ��Ұ� �ΰ� �̻��϶� ������ �߿�(���� ������ �����ؾ���)
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

