using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Chat.Demo;

public class PlayerMove : MonoBehaviourPun
{
    [SerializeField] TestUI_Chat chatUI;
    private PlayerStat stat;
    private SpeedItem speedItem;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;

    private Vector3 moveDir;

    private float curSpeed;
    private bool isChatting = false;
    public bool IsChatting { get { return isChatting; } set { isChatting = value; } }


    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
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
        transform.Rotate(moveDir, Space.World);
    }


    void Move()
    {
        curSpeed = stat.Speed;

        Vector3 vecFor = new Vector3(moveDir.x, 0, moveDir.z).normalized;
        Vector3 vecRb = rb.position;
        rb.MovePosition(vecRb + vecFor * 5 * Time.fixedDeltaTime);

        if (moveDir.sqrMagnitude >= 0.01f)
            transform.rotation = Quaternion.LookRotation(moveDir);
    }

    public void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;

        if (IsChatting == true)
        {
            moveDir.x = 0;
            moveDir.z = 0;
        }

        else if (IsChatting == false)
        {
            if (!stat.IsAlive)
                return;

            if (stat.IsAlive)
            {
                if (moveDir.x > 0 || moveDir.z > 0 || moveDir.x < 0 || moveDir.z < 0)
                    animator.SetBool("Move", true);
                else if (moveDir.x == 0 && moveDir.z == 0)
                    animator.SetBool("Move", false);
                else
                    animator.SetBool("Move", false);

                // 대각선 막기
                if (moveDir.z == 0)
                    return;
                else if (moveDir.z > 0 && moveDir.x > 0 || moveDir.x < 0)
                    moveDir.x = 0;
                else if (moveDir.z < 0 && moveDir.x > 0 || moveDir.x < 0)
                    moveDir.x = 0;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

}
