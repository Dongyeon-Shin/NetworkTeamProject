using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private PlayerStat stat;
    private CharacterController controller;

    private Vector3 moveDir;

    private void Awake()
    {
        stat = new PlayerStat();
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
        transform.Rotate(moveDir, Space.World);
    }

    void Move()
    {
        Vector3 vecFor = new Vector3(moveDir.x, 0, moveDir.z).normalized;
        controller.Move(vecFor * moveSpeed * Time.deltaTime);
        if (moveDir.sqrMagnitude >= 0.01f)
            transform.rotation = Quaternion.LookRotation(moveDir);
    }

    public void OnSetting(InputValue value)
    {
        // GameManager.UI.ShowInGameUI<InGameUI>("");   // 이후 추가
    }

    public void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    public void AddSpeed()
    {

    }

}
