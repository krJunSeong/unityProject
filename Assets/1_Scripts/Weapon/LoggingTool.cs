using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingTool : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitSetting()
    {
        base.InitSetting();
    }

    public override void Use()
    {
        base.Use();
        bodyCollider.enabled = true;
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
        if (other.transform.tag == "Tree")
            other.gameObject.GetComponent<ILoggingAble>().Cut(damage);
    }
}