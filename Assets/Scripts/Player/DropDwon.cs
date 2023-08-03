using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropDwon : MonoBehaviour
{

    private Bomb bomb;

    private void Awake()
    {
        bomb = GameManager.Resource.Load<Bomb>("Prefeb/Bomb");
    }
    private void OnFire(InputValue value)
    {
        DropB();
    }


    private void DropB()
    {
        GameManager.Resource.Instantiate(bomb, transform.position, transform.rotation);
        // 이후 네트워크 식 만들기로 변경
    }
}
