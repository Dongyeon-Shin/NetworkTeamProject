using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Bomb bomb;
    private Explosion explosion;

    private Vector3 vecDir;
    private Vector3 vecDir2;

    private Transform dir;

    [SerializeField] int bombAmount;
    [SerializeField] int bombRemaining;
    [SerializeField] float explosionTime;
    [SerializeField] int explosionlength;

    // Explosion
    [SerializeField] LayerMask explosionMask;
    [SerializeField] float explosionDuration = 1.0f;
    [SerializeField] int explosionRadius = 1;



    private void Awake()
    {
        bomb = GameManager.Resource.Load<Bomb>("Assets/WorkSpace/YoungsinLee/Resource/Bomb/Bomb");
        explosion = GameManager.Resource.Load<Explosion>("Assets/WorkSpace/YoungsinLee/Resource/Bomb/Explosion");


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
        
        // 폭발 범위 디펜스게임에서 참고하기 (벽일경우 폭발Renderer가 겹치지 않게 하기
        //if (Physics.OverlapBox(position, Vector3.one / 2f, 0f, explosionMask))
        //{
        //    return;
        //}

        GameManager.Resource.Instantiate(explosion, position, Quaternion.identity);
        
        
        explosion.SetActiveRenderer(length > 1 ? explosion.mid : explosion.end);
        explosion.SetDirextion(direction);
        explosion.DestrotyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }


    IEnumerator ExplosionRoutine()
    {
        bombRemaining--;
        yield return new WaitForSeconds(explosionTime);

        Explode(vecDir, Vector3.up, explosionlength); // 이후 수정필요
        Explode(vecDir, Vector3.right, explosionlength);
        Explode(vecDir, Vector3.left, explosionlength);
        Explode(vecDir, Vector3.down, explosionlength);
       
        GameManager.Resource.Destroy(gameObject);
        bombRemaining++;
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
