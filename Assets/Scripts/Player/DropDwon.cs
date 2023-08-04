using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropDwon : MonoBehaviour, IEventListener
{

    private Bomb bomb;
    private PlayerStat stat;

    [SerializeField] int bombAmount;
    [SerializeField] int bombRemaining;

    private void Awake()
    {
        bomb = GameManager.Resource.Load<Bomb>("Prefab/Bomb");
    }
    private void OnFire(InputValue value)
    {
        DropB();
    }

    private void OnEnable()
    {
        bombRemaining = bombAmount;
        GameManager.Event.AddListener(EventType.Explode, this);
    }

    private void DropB()
    {
        if (bombRemaining == 0)
            return;
        GameManager.Resource.Instantiate(bomb, CheckStandingBlockPosition(), transform.rotation);

        bombRemaining--;
        // ���� ��Ʈ��ũ �� ������ ����
    }

    private Vector3 CheckStandingBlockPosition()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 2f, 1);
        return new Vector3(hitInfo.transform.position.x, transform.position.y, hitInfo.transform.position.z);
    }

    public void AddBomb()
    {

        bombAmount++;
    }

    // ��ź ���߽� ���� �߰�
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if (EventType.Explode == eventType)
        {
            bombRemaining++;
        }
    }
}
