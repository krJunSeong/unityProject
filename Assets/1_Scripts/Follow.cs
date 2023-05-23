using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Vector3 OffSet;
    public Vector3 rot;
    float x;
    float y;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKey(KeyCode.LeftAlt)) Cursor.lockState = CursorLockMode.None;
       // else Cursor.lockState = CursorLockMode.Locked;

        transform.position = target.position + OffSet;
        rot.x += Input.GetAxisRaw("Mouse X");
        rot.y += Input.GetAxisRaw("Mouse Y");
        rot.z = 0.0f;

        transform.rotation = Quaternion.Euler(-rot.y, rot.x , 0);
    }
}
