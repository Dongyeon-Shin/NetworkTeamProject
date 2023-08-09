using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : PassiveItem
{
    protected override void CoeffiCient()
    {
        coefficient = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(photonView.IsMine)
            other.GetComponent<PlayerStat>().Power = coefficient;
            other.GetComponent<PlayerStat>().ItemInterfaceSet();
            Destroy(gameObject);
            // ���� ���� ���� ��� �Ʒ��� ���
            //Destroy();
        }
    }
}
