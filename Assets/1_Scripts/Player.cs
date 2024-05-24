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
    bool isAttack;      // 콤보 어택을 위한 변수
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
    float attackDelay = 0.3f;   // 공격 간 딜레이
    int maxComboCount = 5;      // 최대 콤보 카운트
    int curComboCount = 0;    // 현재 콤보카운트
    int defaultComboCnt = 0;
    int testCnt = 0;
    float[] animLenths = new float[6];
    bool isComboReserve = false;

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
        for (int i = 0; i < weaponList.Length; i++)
            if (weaponList[i].gameObject.activeSelf) weapon = weaponList[i];

        // 공격 애니메이션의 클립 길이를 갖고 오는 부분
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        animLenths[0] = 0f; // 가독성을 위해 0번 자리 안 쓰고 attack 1 ~5 까지 [1] ~ [5]로 쓸 것임
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
           // Debug.Log("게임매니저가 null 입니다.");
            //return;
        }
        anim.SetBool("isComboReserve", isComboReserve);

        h = curComboCount > defaultComboCnt? 0 : Input.GetAxisRaw("Horizontal");
        v = curComboCount > defaultComboCnt? 0 : Input.GetAxisRaw("Vertical");

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
        if(isAttack || isComboReserve) { h = 0; v = 0; return; }

        isRun = Input.GetButton("Run");

        moveSpeed = isRun? 7.0f : 5.0f;

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
        // 현재 콤보 카운트를 기준으로 콤보를 초기화
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
        // Attack 버튼을 누르고 && 어택 딜레이 가능한 시간인지 체크
        if (Input.GetButtonDown("Attack") && CheckAttackDelay())
        {
            AttackExtention();
        }
        CheckComboDelay();
        // 콤보 유지 시간 초과 시 콤보 초기화
        // time.time, 게임이 시작된 이후의 전체 시간(초)을 반환하는 Unity에서 제공하는 변수, 이 값은 게임이 시작되면 0부터 시작하여 지속적으로 증가
        // AttackAttempt에서 lastAttackTime을 갱신해줘서, 15초(현재 시간) - 12초(마지막 작동한 시간) > 0.3초 이런식으로 작동함
        //if (Time.time - lastAttackTime > attackDelay && isAttack)
        //{
        //    ResetCombo();
        //}
    }

    void AttackExtention()
    {
        //1-1) 플레이어는 콤보 카운트를 갖고, 매 공격 버튼시 콤보카운트를 높인다
        curComboCount = (curComboCount % maxComboCount) + 1; //  1 ~ 5 1 2 3 4 0 -> 1
        anim.SetTrigger("Attack" + curComboCount.ToString());

        //1-2) 무기 콜라이더 Off -> On 해줘서 Trigger 다시 작동
        UseWeapon();
        
        // 1-3) 마지막 공격 시간 갱신
        lastAttackTime = Time.time;
        isAttack = true;
    }

    bool CheckAttackDelay()
    {
        // 공격 할 수 있는 딜레이 시간이 됐는지 체크하는 함수
        // animLenths [1] [2] [3] [4] [5] 만 쓴다. [0] 안 씀
        return Time.time - lastAttackTime > (animLenths[curComboCount] * 0.5f); // 현재시간 - 마지막에 공격한 시간 > 현재 애니메이션의 일정 수치를 지났을 때 가능.
    }

    void CheckComboDelay()
    {   // 공격이 끝났는지 체크하는 함수
        // 마지막 공격 키가 애니메이션 길이보다 오래됐을 경우 공격이 끝났다고 판단
        if (isAttack && Time.time - lastAttackTime > animLenths[curComboCount] * 0.8f)
        {
            //Debug.Log($"{++testCnt}번 작동!");
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
        Debug.Log("Player가 공격당했다!");
        //status.hp -= Random.Range(status.armor - 3, status.armor + 1);
    }
}
