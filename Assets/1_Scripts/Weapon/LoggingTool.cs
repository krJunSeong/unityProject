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
        // UseEnd�Լ��� Player�� ResetCombo���� �۵��Ѵ�.
        // ResetCombo�� �ִϸ��̼� �Լ���, �ִϸ��̼� ���� �� �۵��Ѵ�
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