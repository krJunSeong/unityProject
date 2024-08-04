using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject nearObj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interaction") && nearObj != null) 
            nearObj.GetComponent<IInteractionable>()?.Interaction();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object") nearObj = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == nearObj) nearObj = null; 
    }
    
}
