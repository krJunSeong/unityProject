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
    KeyCode[] numbers = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
                          //KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
                          //KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};
    Vector3 moveVec3;

    // -----------------------------------------
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject scanObject;
    [SerializeField] GameObject nearObject;

    Rigidbody rigid;
    Animator anim;
    // -------------Weapon ---------------
    [SerializeField] Weapon weapon;
    [SerializeField] Weapon[] weaponList;

    // -------------- Item ----------------
    Inventory inventory = new Inventory();

    // ----------------- combo Attack ----------------------
    float attackDelay = 0.3f;    // ���� �� ������
    int maxComboCount = 5;       // �ִ� �޺� ī��Ʈ
    bool isComboReserve = false;

    private int currentComboCount = 0; // ���� �޺� ī��Ʈ
    private float lastAttackTime = 0f; // ������ ���� �ð�

    void Start()
    {
        Init();
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

    void Init()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        DontDestroyOnLoad(this);

        // weapon ã��
        // Gameobject active�� �̷���? �� list�� ������ �ɱ�?
        for (int i = 0; i < weaponList.Length; i++)
            if (weaponList[i].gameObject.activeSelf) weapon = weaponList[i];
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

        for(int i = 0; i < numbers.Length; i++)
        {
            if (Input.GetKeyDown(numbers[i])) ChangeWeapon(i);
        }
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
    void ChangeWeapon(int num)
    {
        // ���� ���⸦ �������� ����
        if (weapon == weaponList[num]) return;

        // ���⸦ �ٲٴ� �Լ�
        weapon.gameObject.SetActive(false);
        weapon.enabled = false;

        weapon = weaponList[num];

        weapon.gameObject.SetActive(true);
        weapon.GetComponent<Weapon>().enabled = true;

        // ���⸦ �ٲ��� �� maxCount�� ���⿡�� �޾ƿ� �ʿ䰡 �ִ�
        ZeroResetCombo();
        GetSetting(weapon);
    }

    void GetSetting(Weapon weapon)
    {
        attackDelay = weapon.GetDelayTime();
        maxComboCount = weapon.GetMaxCombo();
        currentComboCount = 0;

        Debug.Log($"maxComboCount: {maxComboCount}");
    }
    // -------------------------------------- Attack -------------------------
    void Attack()
    {
        // ���� ��ư�� ������ �� �޺� ���� �õ�
        if (Input.GetButtonDown("Attack") && CheckAttackDelay())
        {
            ExcuteAttack();
        }

        // �޺� ���� �ð� �ʰ� �� �޺� �ʱ�ȭ
        // time.time, ������ ���۵� ������ ��ü �ð�(��)�� ��ȯ�ϴ� Unity���� �����ϴ� ����, �� ���� ������ ���۵Ǹ� 0���� �����Ͽ� ���������� ����
        // AttackAttempt���� lastAttackTime�� �������༭, 15��(���� �ð�) - 12��(������ �۵��� �ð�) > 0.3�� �̷������� �۵���
        //if (Time.time - lastAttackTime > attackDelay && isAttack)
        //{
        //    ResetCombo();
        //}
    }

    bool CheckAttackDelay()
    {
        return Time.time - lastAttackTime > attackDelay;
    }
    void ExcuteAttack()
    {
        // ������� �Դٸ� ���� ������ �ð��� ������, ���ù�ư�� ���� ����
        // �������ε� ���� �����̽ð� ������, ���ù�ư�� �����ٸ� ���� �޺����� �� ������ �Ǵ�.
        if (isAttack) isComboReserve = true;

        currentComboCount = ((currentComboCount + 1) % (maxComboCount+1));

        Debug.Log($"currentComboCount:{currentComboCount}");

        isAttack = true;
        anim.SetInteger("comboCount", currentComboCount);
        anim.SetBool("isAttack", isAttack);

        // ù �����̶�� ù ���� �ִϸ��̼� Ʈ���Ÿ� �۵�
        if(currentComboCount == 1) anim.SetTrigger("trStartAttack");

        weapon.GetComponent<Weapon>().Use();
        lastAttackTime = Time.time;

        //Debug.Log("Attack" + currentComboCount);
    }
    public void ResetCombo(int i)
    {
        if (isComboReserve) return;
        if (currentComboCount != i) return;

        //Debug.Log("ResetCombo i: " + i + " curCombo: " + currentComboCount);

        currentComboCount = 0;
        isAttack = false;
        isComboReserve = false;

        anim?.SetInteger("comboCount", currentComboCount);
        anim?.SetBool("isAttack", isAttack);

        weapon?.UseEnd();
    }

    private void ZeroResetCombo()
    {
        // ���� �޺� ī��Ʈ�� �������� �޺��� �ʱ�ȭ
        currentComboCount = 0;
        isAttack = false;
        isComboReserve = false;

        anim?.SetInteger("comboCount", currentComboCount);
        anim?.SetBool("isAttack", isAttack);

        weapon?.UseEnd();
    }
    public void ResetReserveComobo() { isComboReserve = false; }

    // ------------------------------------------------
    public void AddItem(string name, int num)
    {
        inventory.AddItem(name, num);
    }

    public int GetItemInInventory(string name)
    {
        return inventory.GetItemInInventory(name);
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
