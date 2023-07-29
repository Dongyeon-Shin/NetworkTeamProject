using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour
{

    [SerializeField] Bomb bomb;     // 폭탄은 이후 변경 

   
   private void OnFire(InputValue value)
   {
        DropB();
   }


    private void DropB() 
    {
        GameManager.Resource.Instantiate<Bomb>("Bomb", transform.position, transform.rotation);
        // 이후 네트워크 식 만들기로 변경
    }
   
   
}
