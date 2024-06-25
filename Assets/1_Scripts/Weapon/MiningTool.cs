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
        // UseEnd�Լ��� Player�� ResetCombo���� �۵��Ѵ�.
        // ResetCombo�� �ִϸ��̼� �Լ���, �ִϸ��̼� ���� �� �۵��Ѵ�
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
