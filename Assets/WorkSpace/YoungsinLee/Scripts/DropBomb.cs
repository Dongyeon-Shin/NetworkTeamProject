using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviourPun, IEventListener
{
    private TastBomb bombPrefab;
    private TestStat stat;

    private LayerMask de;
    private Vector3 groundDir;

    private int curBomb;
    private PlayerInput playerInput;

    private void Awake()
    {
        if (!photonView.IsMine)
            Destroy(playerInput);
        stat = GetComponent<TestStat>();
        bombPrefab = GameManager.Resource.Load<TastBomb>("Prefab/Bomb");
    }

    private void OnFire(InputValue value)
    {
         Drop();
    }

    private void OnEnable()
    {
        //curBomb = stat.Bomb;
        GameManager.Event.AddListener(EventType.Explode, this);
    }

    private void CheckBomb()
    {
        if (stat.Bomb >= 6)
            return;

    }

    private Vector3 GroundChack()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + 0.3f * Vector3.up, Vector3.down * 0.8f, Color.red);
        if (Physics.Raycast(transform.position + 0.3f * Vector3.up, Vector3.down, out hit, 0.8f))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                groundDir = new Vector3(hit.collider.gameObject.transform.position.x, 0, hit.collider.gameObject.transform.position.z);
                return groundDir;
            }
            else
            {
                Debug.Log("밑에 오브젝트가 암것도 없음");
                return Vector3.zero;
            }
        }
        else
        {
            Debug.Log("암것도 안부딪힘");
            return Vector3.zero;
        }
    }

    private void Drop()
    {
        if (curBomb == 0)
            return;
        //GameManager.Resource.Instantiate(bomb, GroundChack(), transform.rotation);
        curBomb--;

        // 네트워크 식
        photonView.RPC("RequestCreateBomb", RpcTarget.MasterClient, transform.position, transform.rotation);

    }

    [PunRPC]
    private void RequestCreateBomb(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        // 반장이 직접 판단하지 않고 서버를 한번 걸치는 식으로 서버를 통해 판단하게 설정(공평성을 위해)
        photonView.RPC("ResultCreateBomb", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);

    }


    [PunRPC]
    private void ResultCreateBomb(Vector3 position, Quaternion rotation, float sentTime, Player player)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);

        TastBomb bomb = Instantiate(bombPrefab, position, rotation); // 값을 정해서 보내면 더욱 정확한 타이밍을 맞출수있음
       // bomb.SetPlayer(player);
       // bomb.ApplyLag(lag);
        Debug.Log($"{photonView.Owner.NickName}발싸!");
    }
    

    // 폭탄 폭발시 갯수 추가
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if(EventType.Explode == eventType)
        {
            curBomb++;
        }
    }

    // 폭탄 관련 아이템 함수들 

}
