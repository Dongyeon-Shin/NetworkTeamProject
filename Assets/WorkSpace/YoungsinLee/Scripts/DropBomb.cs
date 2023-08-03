using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour
{
    private Bomb bomb;


    private void Awake()
    {
        bomb = GameManager.Resource.Load<Bomb>("Prefab/Bomb");
    }
    private void OnFire(InputValue value)
    {
         DropB();
    }
    
    
    private void DropB() 
    {
        GameManager.Resource.Instantiate(bomb, transform.position, transform.rotation);
        // ���� ��Ʈ��ũ �� ������ ����
    }
   
   
}
