using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : BaseUI
{
    [SerializeField] Transform followTarget;
    public Vector2 follwOffset;
    protected override void Awake()
    {
        base.Awake();
    }
    private void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + (Vector3)follwOffset;
        }
    }

    public void SetTarget(Transform traget)
    {
        this.followTarget = traget;
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + (Vector3)follwOffset;
        }
    }

    public void SetOffSet(Vector2 traget)
    {
        this.follwOffset = traget;
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + (Vector3)follwOffset;
        }
    }
}
