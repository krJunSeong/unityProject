using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmEnterObj : MonoBehaviour, IInteractionable
{
    [SerializeField] Transform trf;
    [SerializeField] GameObject[] uies;

    public bool Interaction()
    {
        Camera.main.transform.position = trf.position;
        Camera.main.transform.rotation = trf.rotation;
        FarmManager.Instance.OpenSeedInventory();
        
        // foreach(var g in uies) g.SetActive(false);

        return true;
    }
}