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
    float attackDelay = 0.3f;    // 공격 간 딜레이
    int maxComboCount = 5;       // 최대 콤보 카운트
    bool isComboReserve = false;

    private int currentComboCount = 0; // 현재 콤보 카운트
    private float lastAttackTime = 0f; // 마지막 공격 시간

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

        // weapon 찾기
        // Gameobject active가 이런데? 왜 list에 들어오는 걸까?
        for (int i = 0; i < weaponList.Length; i++)
            if (weaponList[i].gameObject.activeSelf) weapon = weaponList[i];
    }
    void GetInput()
    {
        if (gameManager == null)
        {
           // Debug.Log("게임매니저가 null 입니다.");
            //return;
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // 나중에 게임매니저 연결하고 나서 이걸로 바꿔야 함
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
        // 대안: rigidbody.velocity = moveVec3

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
        // 같은 무기를 눌렀으면 무시
        if (weapon == weaponList[num]) return;

        // 무기를 바꾸는 함수
        weapon.gameObject.SetActive(false);
        weapon.enabled = false;

        weapon = weaponList[num];

        weapon.gameObject.SetActive(true);
        weapon.GetComponent<Weapon>().enabled = true;

        // 무기를 바꿨을 때 maxCount를 무기에서 받아올 필요가 있다
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
        // 공격 버튼이 눌렸을 때 콤보 공격 시도
        if (Input.GetButtonDown("Attack") && CheckAttackDelay())
        {
            ExcuteAttack();
        }

        // 콤보 유지 시간 초과 시 콤보 초기화
        // time.time, 게임이 시작된 이후의 전체 시간(초)을 반환하는 Unity에서 제공하는 변수, 이 값은 게임이 시작되면 0부터 시작하여 지속적으로 증가
        // AttackAttempt에서 lastAttackTime을 갱신해줘서, 15초(현재 시간) - 12초(마지막 작동한 시간) > 0.3초 이런식으로 작동함
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
        // 여기까지 왔다면 공격 딜레이 시간은 지났고, 어택버튼을 누른 상태
        // 공격중인데 공격 딜레이시간 지났고, 어택버튼을 눌렀다면 다음 콤보공격 할 의지로 판단.
        if (isAttack) isComboReserve = true;

        currentComboCount = ((currentComboCount + 1) % (maxComboCount+1));

        Debug.Log($"currentComboCount:{currentComboCount}");

        isAttack = true;
        anim.SetInteger("comboCount", currentComboCount);
        anim.SetBool("isAttack", isAttack);

        // 첫 공격이라면 첫 공격 애니메이션 트리거만 작동
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
        // 현재 콤보 카운트를 기준으로 콤보를 초기화
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
