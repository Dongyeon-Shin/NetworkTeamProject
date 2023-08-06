using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviourPun, IEventListener
{
    public int tastBomb;


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
        bombPrefab = GameManager.Resource.Load<TastBomb>("Bomb/Bomb");
    }

    private void OnFire(InputValue value)
    {
         Drop();
    }

    private void OnEnable()
    {
        curBomb = tastBomb;
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
                Debug.Log("�ؿ� ������Ʈ�� �ϰ͵� ����");
                return Vector3.zero;
            }
        }
        else
        {
            Debug.Log("�ϰ͵� �Ⱥε���");
            return Vector3.zero;
        }
    }

    private void Drop()
    {
        if (curBomb == 0)
            return;
        photonView.RPC("RequestCreateBomb", RpcTarget.MasterClient, transform.position, transform.rotation);
        //GameManager.Resource.Instantiate(bomb, GroundChack(), transform.rotation);
        curBomb--;
        // ��Ʈ��ũ ��
    }

    [PunRPC]
    private void RequestCreateBomb(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        // ������ ���� �Ǵ����� �ʰ� ������ �ѹ� ��ġ�� ������ ������ ���� �Ǵ��ϰ� ����(������ ����)
        photonView.RPC("ResultCreateBomb", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);
    
    }


    [PunRPC]
    private void ResultCreateBomb(Vector3 position, Quaternion rotation, float sentTime, Player player)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);
        position = GroundChack();
        Instantiate(bombPrefab, position, rotation); // ���� ���ؼ� ������ ���� ��Ȯ�� Ÿ�̹��� ���������
       // bomb.SetPlayer(player);
        Debug.Log($"{photonView.Owner.NickName}�߽�!");
    }

    

    // ��ź ���߽� ���� �߰�
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if(EventType.Explode == eventType)
        {
            curBomb++;
        }
    }

    // ��ź ���� ������ �Լ��� 

}
