using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {

    }
    
    protected override void InitSetting()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bodyCollider = GetComponent<Collider>();
    }
}
