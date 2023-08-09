using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviourPun, IExplosiveReactivable
{
    private TestStat stat;
    private SpeedItem speedItem;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;

    private Vector3 moveDir;

    private float curSpeed;


    private void Awake()
    {
        stat = GetComponent<TestStat>();
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

        rb.MovePosition(vecRb + vecFor * 5 * Time.deltaTime);
        if (moveDir.sqrMagnitude >= 0.01f)
            transform.rotation = Quaternion.LookRotation(moveDir);


    }

    public void OnSetting(InputValue value)
    {
        // GameManager.UI.ShowInGameUI<InGameUI>("");   // 이후 추가
    }

    public void OnChatting() 
    {
        
    }

    public void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
        if (moveDir.x > 0 || moveDir.z > 0 || moveDir.x < 0 || moveDir.z < 0)
        {
            animator.SetBool("Move", true);
        }
        
        else if (moveDir.x == 0 && moveDir.z == 0)
        {
            animator.SetBool("Move", false);
        }

        else
        {
            animator.SetBool("Move", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            other.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            other.isTrigger = false;
    }
    public void ExplosiveReact(Bomb bomb)
    {
        // 죽는 애니메이션 실행
        PhotonNetwork.Destroy(photonView);
    }
}
