using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IMoveAble
{
    Vector2 movement = new Vector2();
    Animator animator;

    public float h { get; set; }
    public float v { get; set; }
    bool isRun;
    private float moveSpeed = 0.0f;
    Vector3 moveVec3;
    Rigidbody rigid;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        isRun = Input.GetButton("Run");

        moveSpeed = isRun ? 7.0f : 5.0f;

        moveVec3 = new Vector3(h, 0, v).normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec3);

        transform.LookAt(transform.position + moveVec3);
        // ´ë¾È: rigidbody.velocity = moveVec3

        float spd = new Vector2(h, v).normalized.magnitude;
        if (!isRun) spd /= 2;
        animator.SetFloat("MoveSpeed", spd);
    }


}
