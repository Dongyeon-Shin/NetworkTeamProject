using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviourPun 
{
    private PlayerStat stat;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector3 moveDir;
    private PlayerUI playerChat;
    private BoxCollider boxCollider;
    private float curSpeed;


    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        playerChat = GetComponent<PlayerUI>();
        boxCollider = GetComponent<BoxCollider>();
        if (!photonView.IsMine)
            Destroy(playerInput);
    }

    private void OnEnable()
    {
        curSpeed = stat.Speed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        curSpeed = stat.Speed;

        Vector3 vecFor = new Vector3(moveDir.x, 0, moveDir.z).normalized;
        Vector3 vecRb = rb.position;
        rb.MovePosition(vecRb + vecFor * 5 * Time.fixedDeltaTime);

        if (moveDir.sqrMagnitude >= 0.01f)
            transform.rotation = Quaternion.LookRotation(moveDir);

        transform.Rotate(moveDir, Space.World);
    }

    public void OnMove(InputValue value)
    {
        if (stat.IsAlive)
        {
            moveDir.x = value.Get<Vector2>().x;
            moveDir.z = value.Get<Vector2>().y;



            if (playerChat.IsChatting == true || playerChat.IsSetting == true)
            {
                moveDir.x = 0;
                moveDir.z = 0;
            }

            else if (playerChat.IsChatting == false || playerChat.IsSetting == true)
            {
                if (moveDir.x > 0 || moveDir.z > 0 || moveDir.x < 0 || moveDir.z < 0)
                    animator.SetBool("Move", true);
                else if (moveDir.x == 0 && moveDir.z == 0)
                    animator.SetBool("Move", false);
                else
                    animator.SetBool("Move", false);

                // �밢�� ����
                if (moveDir.z == 0)
                    return;
                else if (moveDir.z > 0 && moveDir.x > 0 || moveDir.x < 0)
                    moveDir.x = 0;
                else if (moveDir.z < 0 && moveDir.x > 0 || moveDir.x < 0)
                    moveDir.x = 0;
            }
        }
        
    }
}
