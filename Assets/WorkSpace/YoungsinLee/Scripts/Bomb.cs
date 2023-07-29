using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject bomb;
    private GameObject explosinPrefeb;

    private Vector3 vecDir;
    private Vector3 vecDir2;

    private Transform dir;

    [SerializeField] int bombAmount;
    [SerializeField] int bombRemaining;
    [SerializeField] float explosionTime;
    [SerializeField] int explosionlength;



    private void Awake()
    {
        bomb = GameManager.Resource.Load<GameObject>("Assets/WorkSpace/YoungsinLee/Resource/Bomb/Bomb");
        explosinPrefeb = GameManager.Resource.Load<GameObject>("Assets/WorkSpace/YoungsinLee/Resource/Bomb/Explosion"); // 이후 폭발 만들기;
        vecDir = new Vector3(transform.position.x, 0, transform.position.z);
        vecDir2 = new Vector2(transform.position.x, transform.position.z);
        StartCoroutine(ExplosionRoutine());
    }

    private void OnEnable()
    {
        bombRemaining = bombAmount;
    }

    public void Explode(Vector3 position, Vector3 direction, int length)
    {
        if (length <= 0)
            return;

        position += direction;
        GameManager.Resource.Instantiate(explosinPrefeb, position, Quaternion.identity);
        // Explosion 클래스에서 애니메이션, 생성/삭제등 기능구현
        // 여기서 length와 연동되는 함수 구현(범위)
        Explode(position, direction, length-1);

    }


    IEnumerator ExplosionRoutine()
    {
        Explode(vecDir, vecDir.normalized, explosionlength); // 이후 수정필요
        bombRemaining--;
        yield return new WaitForSeconds(explosionTime);
        GameManager.Resource.Destroy(gameObject);
        bombRemaining--;
    }

    // 폭탄갯수증가 아이템 먹을시 발동
    public void AddBomb()
    {
        bombAmount++;
        bombRemaining++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
