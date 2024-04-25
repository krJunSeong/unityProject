using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningTool : Weapon
{
    private void Awake()
    {
        base.InitSetting();
    }

    public override void Use()
    {
        base.Use();
        bodyCollider.enabled = true;

        Debug.Log("use");
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
        Debug.Log("collision used : " + collision.gameObject.name);

        if (collision.transform.tag == "Stone")
        { collision.gameObject.GetComponent<ILoggingAble>().Used(damage);
        }
           
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger loged : " + other.gameObject.name);

        if (other.transform.tag == "Stone")
        {
            other.gameObject.GetComponent<ILoggingAble>().Used(damage);
        }
            
    }
}
