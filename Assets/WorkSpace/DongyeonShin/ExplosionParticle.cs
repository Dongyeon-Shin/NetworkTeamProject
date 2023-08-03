using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Resource.Destroy(gameObject, 3f);
    }
}
