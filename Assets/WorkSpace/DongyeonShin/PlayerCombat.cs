using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviourPun, IExplosiveReactivable
{
    private PlayerStat stat;
    private Animator animator;
    private Stack<Bomb> plantingBombs = new Stack<Bomb>();

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
    }

    private void OnFire(InputValue value)
    {
        if (stat.IsAlive && stat.Bomb > plantingBombs.Count)
        {
            Debug.Log("��ź!");
            photonView.RPC("PlantABomb", RpcTarget.AllViaServer, CheckStandingBlockPosition(), stat.Power, stat.PlayerNumber);
        }
    }

    [PunRPC]
    private void PlantABomb(Vector3 position, int explosivePower, int playerNumber)
    {
        Bomb plantedBomb = GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), position, transform.rotation).GetComponent<Bomb>();
        plantedBomb.ExplosivePower = explosivePower;
        if (playerNumber == stat.PlayerNumber)
        {
            plantingBombs.Push(plantedBomb);
            StartCoroutine(RetrieveBombRoutine(plantedBomb));
        }
        else
        {
            StartCoroutine (plantedBomb.ExplodeRoutine());
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

    public void ExplosiveReact()
    {
        //TODO: �÷��̾� �ǰݽ� ����
        animator.SetBool("Die", true);
        stat.IsAlive = false;
    }

    IEnumerator DyingRoutine()
    {
        yield return new WaitForSeconds(2f);
    }

}
