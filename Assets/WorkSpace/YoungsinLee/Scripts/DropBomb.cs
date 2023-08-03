using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour
{
    private Bomb bomb;
    [SerializeField] int bombAmount;
    [SerializeField] int bombRemaining;

    private void Awake()
    {
        bomb = GameManager.Resource.Load<Bomb>("Prefeb/Bomb");
    }
    private void OnFire(InputValue value)
    {
         DropB();
    }

    private void OnEnable()
    {
        bombRemaining = bombAmount;
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
        bombRemaining++;
    }
}
