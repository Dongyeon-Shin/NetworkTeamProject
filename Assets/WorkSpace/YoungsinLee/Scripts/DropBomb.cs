using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour, IEventListener
{
    private Bomb bomb;
    private PlayerStat stat;

    private LayerMask de;
    private Vector3 groundDir;

    private int maxBomb;
    private int curBomb;
    private int curPower;

    private void Awake()
    {
        stat = new PlayerStat();
        bomb = GameManager.Resource.Load<Bomb>("Prefab/Bomb");
    }

    private void OnFire(InputValue value)
    {
         Drop();
    }

    private void Update()
    {
        if (maxBomb >= 6)
            return;
        if (curPower >= 5)
            return;
    }

    private void OnEnable()
    {
        maxBomb = stat.Bomb;
        curBomb = maxBomb;
        GameManager.Event.AddListener(EventType.Explode, this);
    }

    private void GroundChack()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 0.5f);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                groundDir = new Vector3(hit.collider.gameObject.transform.position.x, 0, hit.collider.gameObject.transform.position.z);
            }
            else
            {
                Debug.Log("�ؿ� ������Ʈ�� �ϰ͵� ����");
            }
        }
        else
        {
            Debug.Log("�ϰ͵� �Ⱥε���");
        }
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void Drop()
    {
        if (curBomb == 0)
            return;
        GameManager.Resource.Instantiate(bomb, transform.position, transform.rotation);
        curBomb--;
        // ���� ��Ʈ��ũ �� ������ ����
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
