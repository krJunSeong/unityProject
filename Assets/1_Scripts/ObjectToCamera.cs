using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToCamera : MonoBehaviour
{
    public Rigidbody rb;
    public Camera cam;
    float h;
    float v;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // Cam z, x
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;


        Vector3 forwardRelative = v * camForward;
        Vector3 rightRelative = h * camRight;

        Vector3 moveDir = forwardRelative + rightRelative;

        // movement
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        transform.forward = new Vector3(moveDir.x, 0, moveDir.z);
    }
}
