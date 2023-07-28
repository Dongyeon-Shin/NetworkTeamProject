using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private CharacterController controller;

    private Vector3 moveDir;

    private float curSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
        transform.Rotate(moveDir, Space.World);
    }

    void Move()
    {
        // Mathf.Lerp() 애니메이션에 부드러운 전환을 넣기위한 정밀작업
        //if (moveDir.magnitude == 0)
        //{
        //    curSpeed = Mathf.Lerp(curSpeed, 0f, 0.5f);
        //    isRun = false;
        //    return;
        //}

        Vector3 vecFor = new Vector3(moveDir.x, 0, moveDir.z).normalized;

        controller.Move(vecFor * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(moveDir);

    }

    void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    
    }
}
