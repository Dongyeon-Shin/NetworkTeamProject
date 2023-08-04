using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class DropDwon : MonoBehaviour, IEventListener
{

    private Bomb bomb;
    private PlayerStat stat;

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
        DropB();
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

    private void DropB()
    {
        if (curBomb == 0)
            return;
        GameManager.Resource.Instantiate(bomb, CheckStandingBlockPosition(), transform.rotation);

        curBomb--;
        // 이후 네트워크 식 만들기로 변경
    }

    private Vector3 CheckStandingBlockPosition()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + 0.3f * Vector3.up, Vector3.down, out hitInfo, 1.8f, 1);
        return new Vector3(hitInfo.transform.position.x, transform.position.y, hitInfo.transform.position.z);
    }

    public void AddBomb()
    {
        maxBomb++;
    }

    // 폭탄 폭발시 갯수 추가
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if (EventType.Explode == eventType)
        {
            curBomb++;
        }
    }
}
