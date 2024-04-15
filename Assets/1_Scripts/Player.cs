using UnityEngine;

public class Player : MonoBehaviour
{
    // -------------------
    private float moveSpeed = 0.0f;
    private float walkSpeed = 5.0f;
    private float runSpeed = 10.0f;
    private float jumpFoce = 10.0f;
    private ForceMode forcemode = ForceMode.Impulse;

    // ---------------- input -------------
    float h;
    float v;

    bool isRun;
    bool isJump;
    bool isAttack;
    bool eDown;
    bool isInteraction = false;

    Vector3 moveVec3;

    // -----------------------------------------
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject scanObject;
    [SerializeField] GameObject nearObject;
    [SerializeField] Weapon weapon;

    Rigidbody rigid;
    Animator anim;

    // ----------------- combo Attack ----------------------
    public float attackDelay = 0.3f;    // ���� �� ������
    public int maxComboCount = 5;       // �ִ� �޺� ī��Ʈ

    private int currentComboCount = 0; // ���� �޺� ī��Ʈ
    private float lastAttackTime = 0f; // ������ ���� �ð�

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Interaction();
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        //transform.position += moveVec3;
        //transform.LookAt(transform.position + moveVec3);

        //transform.position += moveVec3 * moveSpeed * Time.deltaTime;
        moveVec3 = new Vector3(h, 0, v).normalized;
        rigid.MovePosition(rigid.position + moveVec3 * moveSpeed * Time.deltaTime);

        if(new Vector3(h,0,v).normalized.magnitude > 0)
            rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, Quaternion.LookRotation(moveVec3), 0.3f));
    }

    void GetInput()
    {
        if (gameManager == null)
        {
           // Debug.Log("���ӸŴ����� null �Դϴ�.");
            //return;
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // ���߿� ���ӸŴ��� �����ϰ� ���� �̰ɷ� �ٲ�� ��
        //h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        //v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        eDown = Input.GetButtonDown("Interaction");
    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isJump && !isInteraction)
        {
            rigid.AddForce(Vector3.up * jumpFoce, forcemode);
            isJump = true;
        }
    }
    void Move()
    {
        if (isInteraction) return;

        isRun = Input.GetButton("Run");

        if (isRun) moveSpeed = 7.0f;
        else moveSpeed = 5.0f;

        moveVec3 = new Vector3(h, 0, v).normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec3);

        transform.LookAt(transform.position + moveVec3);
        // ���: rigidbody.velocity = moveVec3

        anim.SetBool("isWolk", moveVec3 != Vector3.zero);
        anim.SetBool("isRun", isRun);        
    }
    void Interaction()
    {
        if (eDown)
        {
            gameManager.TalkAction(scanObject);
        }
    }
   void ChangeInteraction()
    {
        isInteraction = !isInteraction;
    }
    void Attack()
    {
        // ���� ��ư�� ������ �� �޺� ���� �õ�
        if (Input.GetButton("Attack") && Time.time - lastAttackTime > attackDelay)
        {
            AttemptComboAttack();
        }

        // �޺� ���� �ð� �ʰ� �� �޺� �ʱ�ȭ
        // time.time, ������ ���۵� ������ ��ü �ð�(��)�� ��ȯ�ϴ� Unity���� �����ϴ� ����, �� ���� ������ ���۵Ǹ� 0���� �����Ͽ� ���������� ����
        // AttackAttempt���� lastAttackTime�� �������༭, 15��(���� �ð�) - 12��(������ �۵��� �ð�) > 0.3�� �̷������� �۵���
        if (Time.time - lastAttackTime > attackDelay && isAttack)
        {
            ResetCombo();
        }
    }
    void AttemptComboAttack()
    {
        Debug.Log("Attack");

        currentComboCount++;
        isAttack = true;
        anim.SetBool("isAttack", isAttack);

        if (currentComboCount == 1) anim.SetTrigger("trDefaultAttack");
        else if(currentComboCount > 1) anim.SetTrigger("trComboAttack");

        GetComponent<Weapon>()?.Use();
        lastAttackTime = Time.time;
    }
    void ResetCombo()
    {
        currentComboCount = 0;
        isAttack = false;
        anim.SetBool("isAttack", isAttack);
    }

    // -------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor") isJump = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            nearObject = other.gameObject;

            if (Input.GetButtonDown("Interaction")) anim.SetTrigger("Interaction");
        }
        else
            scanObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
            nearObject = null;
    }
}
