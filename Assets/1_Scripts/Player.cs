using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpFoce = 10.0f;
    public ForceMode forcemode = ForceMode.Impulse;

    float h;
    float v;

    bool isRun;
    bool eDown;
    bool isInteraction = false;

    Vector3 moveVec3;

    GameObject nearObject;

    Rigidbody rigid;
    Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Jump();
    }

    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        eDown = Input.GetButtonDown("Interaction");
    }
    void Jump()
    {
        if (isInteraction) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpFoce, forcemode);

            Debug.Log("Jump!");
        }
    }
    void Move()
    {
        if (isInteraction) return;

        isRun = Input.GetButton("Run");

        if (isRun) moveSpeed = 7.0f;
        else moveSpeed = 5.0f;

        moveVec3 = new Vector3(h, 0, v).normalized * moveSpeed * Time.deltaTime;
        transform.position += moveVec3;

        transform.LookAt(transform.position + moveVec3);

        anim.SetBool("isWolk", moveVec3 != Vector3.zero);
        anim.SetBool("isRun", isRun);        
    }

   void ChangeInteraction()
    {
        isInteraction = !isInteraction;
        Debug.Log("ChangeInteraction »£√‚!");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            nearObject = other.gameObject;

            if (Input.GetButtonDown("Interaction")) anim.SetTrigger("Interaction");
            Debug.Log("Item Search");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
            nearObject = null;

        Debug.Log("Item exit");
    }

}
