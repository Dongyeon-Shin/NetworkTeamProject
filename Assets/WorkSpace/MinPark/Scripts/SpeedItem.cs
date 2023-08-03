using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PassiveItem
{
    protected override void CoeffiCient()
    {
        coefficient = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStat>().Speed += coefficient;
            Destroy(gameObject);
        }
    }
}
