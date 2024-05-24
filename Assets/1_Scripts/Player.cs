using System.Collections;
using UnityEngine;

public struct CharacterState
{
    public float damage { get; set; }
    public float hp { get; set; }
    public float armor { get; set; }
    public float speed { get; set; }
};

public class Player : MonoBehaviour, IDamageAble
{
    CharacterState status;

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
    bool isAttack;      // �޺� ������ ���� ����
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
    float attackDelay = 0.3f;   // ���� �� ������
    int maxComboCount = 5;      // �ִ� �޺� ī��Ʈ
    int curComboCount = 0;    // ���� �޺�ī��Ʈ
    int defaultComboCnt = 0;
    int testCnt = 0;
    float[] animLenths = new float[6];
    bool isComboReserve = false;

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
        for (int i = 0; i < weaponList.Length; i++)
            if (weaponList[i].gameObject.activeSelf) weapon = weaponList[i];

        // ���� �ִϸ��̼��� Ŭ�� ���̸� ���� ���� �κ�
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        animLenths[0] = 0f; // �������� ���� 0�� �ڸ� �� ���� attack 1 ~5 ���� [1] ~ [5]�� �� ����
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("attack"))
            {
                if(clip.name[clip.name.Length - 1] - '0' < 6)
                {
                    // Attack1  -> 1 -> (int)1
                    animLenths[clip.name[clip.name.Length - 1] - '0'] = clip.length;
                    Debug.Log($"{clip.name} : {clip.length}");
                }
            }
        }
    }
    void GetInput()
    {
        if (gameManager == null)
        {
           // Debug.Log("���ӸŴ����� null �Դϴ�.");
            //return;
        }
        anim.SetBool("isComboReserve", isComboReserve);

        h = curComboCount > defaultComboCnt? 0 : Input.GetAxisRaw("Horizontal");
        v = curComboCount > defaultComboCnt? 0 : Input.GetAxisRaw("Vertical");

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
        if(isAttack || isComboReserve) { h = 0; v = 0; return; }

        isRun = Input.GetButton("Run");

        moveSpeed = isRun? 7.0f : 5.0f;

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
        // ���� �޺� ī��Ʈ�� �������� �޺��� �ʱ�ȭ
        curComboCount = defaultComboCnt;
        isAttack = false;
        isComboReserve = false;

        anim?.SetBool("isAttack", isAttack);
        weapon?.UseEnd();

        GetSetting(weapon);
    }

    void GetSetting(Weapon weapon)
    {
        attackDelay = weapon.GetDelayTime();
        maxComboCount = weapon.GetMaxCombo();
        curComboCount = 0;
    }
    // -------------------------------------- Attack -------------------------
    void Attack()
    {
        // Attack ��ư�� ������ && ���� ������ ������ �ð����� üũ
        if (Input.GetButtonDown("Attack") && CheckAttackDelay())
        {
            AttackExtention();
        }
        CheckComboDelay();
        // �޺� ���� �ð� �ʰ� �� �޺� �ʱ�ȭ
        // time.time, ������ ���۵� ������ ��ü �ð�(��)�� ��ȯ�ϴ� Unity���� �����ϴ� ����, �� ���� ������ ���۵Ǹ� 0���� �����Ͽ� ���������� ����
        // AttackAttempt���� lastAttackTime�� �������༭, 15��(���� �ð�) - 12��(������ �۵��� �ð�) > 0.3�� �̷������� �۵���
        //if (Time.time - lastAttackTime > attackDelay && isAttack)
        //{
        //    ResetCombo();
        //}
    }

    void AttackExtention()
    {
        //1-1) �÷��̾�� �޺� ī��Ʈ�� ����, �� ���� ��ư�� �޺�ī��Ʈ�� ���δ�
        curComboCount = (curComboCount % maxComboCount) + 1; //  1 ~ 5 1 2 3 4 0 -> 1
        anim.SetTrigger("Attack" + curComboCount.ToString());

        //1-2) ���� �ݶ��̴� Off -> On ���༭ Trigger �ٽ� �۵�
        UseWeapon();
        
        // 1-3) ������ ���� �ð� ����
        lastAttackTime = Time.time;
        isAttack = true;
    }

    bool CheckAttackDelay()
    {
        // ���� �� �� �ִ� ������ �ð��� �ƴ��� üũ�ϴ� �Լ�
        // animLenths [1] [2] [3] [4] [5] �� ����. [0] �� ��
        return Time.time - lastAttackTime > (animLenths[curComboCount] * 0.5f); // ����ð� - �������� ������ �ð� > ���� �ִϸ��̼��� ���� ��ġ�� ������ �� ����.
    }

    void CheckComboDelay()
    {   // ������ �������� üũ�ϴ� �Լ�
        // ������ ���� Ű�� �ִϸ��̼� ���̺��� �������� ��� ������ �����ٰ� �Ǵ�
        if (isAttack && Time.time - lastAttackTime > animLenths[curComboCount] * 0.8f)
        {
            //Debug.Log($"{++testCnt}�� �۵�!");
            curComboCount = defaultComboCnt;
            weapon.UseEnd();
            isAttack = false;
        }
    }

    void UseWeapon()
    {
        if(weapon.gameObject.activeSelf) weapon.UseEnd();
        weapon.Use(status.damage);
    }

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

    public void Damaged(float damage)
    {
        Debug.Log("Player�� ���ݴ��ߴ�!");
        //status.hp -= Random.Range(status.armor - 3, status.armor + 1);
    }
}
