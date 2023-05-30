using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject axe;

    [SerializeField]
    Transform characterTrj;

    bool isLogging = false;
    bool isAxe = false;

    float distance = 3.0f;
    RaycastHit rayHit;
    Ray ray;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ray = new Ray();
        ray.origin = characterTrj.position + Vector3.up * 0.5f;
        ray.direction = this.transform.forward;
        
        if(Physics.Raycast(ray.origin, ray.direction, out rayHit, distance))
        {
            Debug.Log(rayHit.collider.gameObject.name);
            //if(rayHit.transform.tag == "Tree" && GetButton)
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
    }
    void EquitAxe()
    {
        if(isAxe) axe.SetActive(true);
    }

    void DoLogging()
    {
        isLogging = true;
        anim.SetBool("isLogging", true);
    }
}
