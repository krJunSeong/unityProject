using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningTool : Weapon
{
    private void Awake()
    {
        base.InitSetting();
    }

    public override void Use(float dam, int curCombo)
    {
        base.Use(dam, curCombo);
    }

    public override void UseEnd()
    {
        // UseEnd함수는 Player의 ResetCombo에서 작동한다.
        // ResetCombo는 애니메이션 함수로, 애니메이션 끝날 때 작동한다
        base.UseEnd();
        bodyCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    { 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Stone")
        {
            other.gameObject.GetComponent<ILoggingAble>().Used(damage);
        }

        if (attackAble && other.tag == "Monster") other.gameObject.GetComponent<IDamageAble>().Damaged(damage, other.ClosestPoint(transform.position));
    }
}
