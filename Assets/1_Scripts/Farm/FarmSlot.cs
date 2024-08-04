using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSlot : Slot
{
    void Open()
    {
        if (currentItem == null) gameObject.SetActive(false);
    }

    void Awake()
    {
        //base.Start();

        //Open();
    }

    private void Update()
    {
        
    }
}