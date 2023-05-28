using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angleScript : MonoBehaviour
{
    public Transform target;
    public GameObject target2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relative = transform.InverseTransformPoint(target.position);
        Vector3 relative2 = target.position - transform.forward;

        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(relative2.x, relative2.z) * Mathf.Rad2Deg;

        transform.Rotate(0, angle, 0);
        Debug.Log(angle);
        target2.transform.position = relative;
    }
}
