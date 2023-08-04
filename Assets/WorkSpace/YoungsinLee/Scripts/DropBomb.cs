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
                Debug.Log("밑에 오브젝트가 암것도 없음");
            }
        }
        else
        {
            Debug.Log("암것도 안부딪힘");
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
        // 이후 네트워크 식 만들기로 변경
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
