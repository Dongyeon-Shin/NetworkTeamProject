using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour, IEventListener
{
    private Bomb bomb;
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
        GameManager.Resource.Instantiate(bomb, transform.position, transform.rotation);
        bombRemaining--;
        // 이후 네트워크 식 만들기로 변경
    }
    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void AddBomb()
    {
        bombAmount++;
    }

    // 폭탄 폭발시 갯수 추가
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if(EventType.Explode == eventType)
        {
            bombRemaining++;
        }
    }
}
