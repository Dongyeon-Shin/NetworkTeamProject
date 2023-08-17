using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviourPun 
{
    private PlayerStat stat;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector3 moveDir;
    private PlayerUI playerChat;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private float curSpeed;
    private bool isTransferable = true;
    [SerializeField]
    private LayerMask obstacleLayerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
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
        if (isTransferable)
        {
            Vector3 vecFor = new Vector3(moveDir.x, 0, moveDir.z).normalized;
            Vector3 vecRb = rb.position;
            rb.MovePosition(vecRb + vecFor * 5 * Time.fixedDeltaTime);
        }

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

    public IEnumerator PassThroughRoutine(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) < 0.6f)
        {
            isTransferable = CheckFront();
            boxCollider.isTrigger = true;
            yield return null;
        }
        boxCollider.isTrigger = false;
        yield return null;
        isTransferable = true;
    }

    private bool CheckFront()
    {
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), transform.forward, 0.3f, obstacleLayerMask))
        {
            return false;
        }
        return true;
    }
}
