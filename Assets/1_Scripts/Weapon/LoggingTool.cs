using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingTool : Weapon
{
    protected override void InitSetting()
    {
        base.InitSetting();
    }

    public override void Use(float dam, int curCombo)
    {
        base.Use(dam + damage, curCombo);
    }

    public override void UseEnd()
    {
        // UseEnd함수는 Player의 ResetCombo에서 작동한다.
        // ResetCombo는 애니메이션 함수로, 애니메이션 끝날 때 작동한다
        base.UseEnd();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tree")
            other.gameObject.GetComponent<ILoggingAble>()?.Used(damage);

        if (attackAble && other.gameObject.tag == "Monster") 
        {
            //if(CheckDamagedList(other.name)) 
                other.gameObject.GetComponent<IDamageAble>()?.Damaged(damage, other.ClosestPoint(transform.position)); 
        }
    }
}