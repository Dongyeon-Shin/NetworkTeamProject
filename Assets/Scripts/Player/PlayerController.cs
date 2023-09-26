using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // 이후 수정 예정
    // [SerializeField] Bomb bomb;
   
   //private void OnFire(InputValue value)
   //{
   //    photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
   //}
   //
   //// 마스터클라이언트 버전(판정이 더 중요할경우)
   //[PunRPC]
   //private void RequestCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
   //{
   //    // 마스터클라이언트 입장(서버)에서 판정을 진행하는 버전임
   //    if (Time.time < lastFireTime + fireCoolTime)
   //        return;
   //    lastFireTime = Time.time;
   //
   //    float sentTime = (float)info.SentServerTime;
   //    // 반장이 직접 판단하지 않고 서버를 한번 걸치는 식으로 서버를 통해 판단하게 설정(공평성을 위해)
   //    photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);
   //
   //}
   //
   //
   //
   //[PunRPC]
   //private void ResultCreateBullet(Vector3 position, Quaternion rotation, float sentTime, Player player)
   //{
   //    bulletCount++;      // 변수 동기화 예시
   //
   //    float lag = (float)(PhotonNetwork.Time - sentTime);
   //
   //    Bullet bullet = Instantiate(bulletPrefeb, position, rotation); // 값을 정해서 보내면 더욱 정확한 타이밍을 맞출수있음
   //    bullet.SetPlayer(player);
   //    bullet.ApplyLag(lag);
   //    Debug.Log($"{photonView.Owner.NickName}발싸!");
   //}
   //
}
