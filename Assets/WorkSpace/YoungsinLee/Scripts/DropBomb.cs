using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropBomb : MonoBehaviour, IEventListener
{
    private Bomb bomb;
    private PlayerStat stat;
    private GameObject ground;

    private LayerMask de;

    private int maxBomb;
    private int curBomb;
    private int curPower;

    private void Awake()
    {
        stat = new PlayerStat();
        bomb = GameManager.Resource.Load<Bomb>("Prefab/Bomb");
    }
    private void OnFire(InputValue value)
    {
         Drop();
    }

    private void OnEnable()
    {
        curPower = stat.Power;
        maxBomb = stat.Bomb;
        curBomb = maxBomb;
        GameManager.Event.AddListener(EventType.Explode, this);
    }

    private void Drop() 
    {
        if (curBomb == 0)
            return;

        
        ground.transform.position = new Vector3(0, 0, 0);
        if (GroundChack())
        {
            GameManager.Resource.Instantiate(bomb, ground.transform.position, transform.rotation);
            curBomb--;
        }
        // ÀÌÈÄ ³×Æ®¿öÅ© ½Ä ¸¸µé±â·Î º¯°æ
    }

    private bool GroundChack()
    {
        RaycastHit hit;
        //ground = hit.transform.gameObject;
        return Physics.Raycast(transform.position, Vector3.down, 1.5f, 0, QueryTriggerInteraction.Ignore);
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }

    // ÆøÅº Æø¹ß½Ã °¹¼ö Ãß°¡
    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if(EventType.Explode == eventType)
        {
            curBomb++;
        }
    }

    // ÆøÅº °ü·Ã ¾ÆÀÌÅÛ ÇÔ¼öµé 

    public void AddBomb()
    {
        if (maxBomb >= 6)
            return;
        else
            maxBomb++;
    }

    public void AddPower()
    {
        if (curPower >= 5)
            return;
        else
            curPower++; ;
    }
}
