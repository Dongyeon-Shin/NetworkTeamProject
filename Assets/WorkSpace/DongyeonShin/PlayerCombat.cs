using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviourPun, IExplosiveReactivable
{
    private PlayerStat stat;
    private Stack<Bomb> plantingBombs = new Stack<Bomb>();
    private bool isAlive;

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
    }

    private void OnFire(InputValue value)
    {
        if (stat.Bomb > plantingBombs.Count)
        {
            photonView.RPC("PlantABomb", RpcTarget.AllViaServer);
            //photonView.RPC("RequestPlantABomb", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void PlantABomb()
    {
        plantingBombs.Push(GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), CheckStandingBlockPosition(), transform.rotation).GetComponent<Bomb>());
        plantingBombs.Peek().ExplosivePower = stat.Power;
        StartCoroutine(RetrieveBombRoutine(plantingBombs.Peek()));
    }

    //[PunRPC]
    //private void RequestPlantABomb()
    //{
    //    plantingBombs.Push(GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), CheckStandingBlockPosition(), transform.rotation).GetComponent<Bomb>());
    //    plantingBombs.Peek().ExplosivePower = stat.Power;
    //    StartCoroutine(RetrieveBombRoutine(plantingBombs.Peek()));
    //    밑의 코드를 바깥에 
    //    if (isLocalPlayer)
    //    photonView.RPC("PlantABomb", RpcTarget.AllViaServer, CheckStandingBlockPosition(), stat.Power);
    //}

    //[PunRPC]
    //private void ResultPlantABomb(Vector3 position, int explosivePower)
    //{
    //    GameManager.Resource.Instantiate(Resources.Load("Prefab/Bomb"), position, Quaternion.identity);
    //}

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
        //TODO: 플레이어 피격시 반응
    }

}
