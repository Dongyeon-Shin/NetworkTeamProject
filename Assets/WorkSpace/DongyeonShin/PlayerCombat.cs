using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviourPun, IExplosiveReactivable
{
    [SerializeField] GameObject deadState;
    [SerializeField]
    private LayerMask playerLayerMask;

    private PlayerStat stat;
    private PlayerMove move;
    private PlayerUI playerUI;
    private Animator animator;
    private Stack<Bomb> plantingBombs = new Stack<Bomb>();
    public int IDNumber { get { return stat.IDNumber; } set { stat.IDNumber = value; } }
    public GameScene GameScene { get { return stat.GameScene; } set { stat.GameScene = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stat = GetComponent<PlayerStat>();
        move = GetComponent<PlayerMove>();
        playerUI = GetComponent<PlayerUI>();
    }

    private void OnFire(InputValue value)
    {
        if (playerUI.IsChatting == true && playerUI.IsSetting == true)
        {
            if (stat.IsAlive && stat.Bomb > plantingBombs.Count)
            {
                Vector3 position = CheckStandingBlockPosition();
                plantingBombs.Push(GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), position, transform.rotation, true).GetComponent<Bomb>());
                CheckPlayers(position);
                if (plantingBombs.Peek().GameScene == null)
                {
                    plantingBombs.Peek().GameScene = stat.GameScene;
                    plantingBombs.Peek().RegeisterBombID();
                }
                int explosivePower = stat.Power;
                plantingBombs.Peek().ExplosivePower = explosivePower;
                StartCoroutine(RetrieveBombRoutine(plantingBombs.Peek()));
                photonView.RPC("PlantABomb", RpcTarget.Others, CheckStandingBlockPosition(), explosivePower, stat.PlayerNumber);
            }
        }
    }

    [PunRPC]
    private void PlantABomb(Vector3 position, int explosivePower, int playerNumber)
    {
        Bomb plantedBomb = GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), position, transform.rotation, true).GetComponent<Bomb>();
        CheckPlayers(position);
        if (plantedBomb.GameScene == null)
        {
            plantedBomb.GameScene = stat.GameScene;
            plantedBomb.RegeisterBombID();
        }
        stat.GameScene.CountBomb();
        plantedBomb.ExplosivePower = explosivePower;
        StartCoroutine(plantedBomb.ExplodeRoutine());
    }

    public void CheckPlayers(Vector3 position)
    {
        Collider[] players = Physics.OverlapSphere(position, 0.5f, playerLayerMask);
        foreach (Collider collider in players)
        {
            StartCoroutine(collider.GetComponent<PlayerMove>().PassThroughRoutine(position));
        }
    }

    IEnumerator RetrieveBombRoutine(Bomb bomb)
    {
        yield return StartCoroutine (bomb.ExplodeRoutine());
        plantingBombs.Pop();
    }

    private Vector3 CheckStandingBlockPosition()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + 0.3f * Vector3.up, Vector3.down, out hitInfo, 1.8f, 1);
        return new Vector3(hitInfo.transform.position.x, transform.position.y, hitInfo.transform.position.z);
    }

    public void ExplosiveReact(int bombIDNumber)
    {
        stat.GameScene.ExplodeABomb(bombIDNumber);
        //TODO: 플레이어 피격시 반응
        stat.IsAlive = false;
        animator.SetBool("Die", true);
        photonView.RPC("DeadSet", RpcTarget.All, stat.PlayerNumber);
    }

    [PunRPC]
    public void DeadSet(int playerNumber)
    {
        if (playerNumber == stat.PlayerNumber)
        {
            stat.IsAlive = false;
            StartCoroutine(DeadRoutine(playerNumber));
        }
    }

    IEnumerator DeadRoutine(int playerNumber)
    {
        yield return new WaitForSeconds(4f);
        deadState.SetActive(false);
    }
}
