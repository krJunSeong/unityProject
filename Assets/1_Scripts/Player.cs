using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public struct CharacterState
{
    public float damage { get; set; }
    public float hp { get; set; }
    public float armor { get; set; }
    public float speed { get; set; }
};

public class Player : MonoBehaviour, IDamageAble
{
    public CharacterState status { get; }

    // -------------------
    private float walkSpeed = 5.0f;
    private float runSpeed = 10.0f;
    private float jumpFoce = 10.0f;
    private ForceMode forcemode = ForceMode.Impulse;

    // ---------------- input -------------

    bool isJump;
    bool isAttack;      // �޺� ������ ���� ����
    bool eDown;
    bool isInteraction = false;
    bool isSturn = false;
    bool isBaatle = false;
    KeyCode[] numbers = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    // -----------------------------------------
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject scanObject;
    [SerializeField] GameObject nearObject;

    Animator animator;
    Rigidbody rigid;
    PlayerMove playerMove;
    PlayerAttack playerAttack;
    // -------------Weapon ---------------
    [SerializeField] Weapon weapon;
    [SerializeField] Weapon[] weaponList;

    public Weapon Weapon => weapon;
    // -------------- Item ----------------
    Inventory inventory = new Inventory();

    // ----------------- combo Attack ----------------------
    float attackDelay = 0.5f;   // ���� �� ������
    int maxComboCount = 5;      // �ִ� �޺� ī��Ʈ
    int curComboCount = 0;      // ���� �޺�ī��Ʈ
    int defaultComboCnt = 0;

    float[] animLenths = new float[6];

    private float lastAttackTime = 0f; // ������ ���� �ð�
    // ---------- ���� ��
    [SerializeField] Image image;

    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        Jump();
        Attack();
        CheckTime();

        image.enabled = weapon.GetBodyCol();
    }

    private void FixedUpdate()
    {
        Move();

        /* Move ����
        transform.position += moveVec3;
        transform.LookAt(transform.position + moveVec3);

        transform.position += moveVec3 * moveSpeed * Time.deltaTime;
        moveVec3 = new Vector3(h, 0, v).normalized;
        rigid.MovePosition(rigid.position + moveVec3 * moveSpeed * Time.deltaTime);
        
        if(new Vector3(h,0,v).normalized.magnitude > 0)
            rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, Quaternion.LookRotation(moveVec3), 0.3f));
        */
    }

    void Init()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();

        DontDestroyOnLoad(this);

        // weapon ã��
        for (int i = 0; i < weaponList.Length; i++)
            if (weaponList[i].gameObject.activeSelf) weapon = weaponList[i];

        // ���� �ִϸ��̼��� Ŭ�� ���̸� ���� ���� �κ�
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        animLenths[0] = 0f; // �������� ���� 0�� �ڸ� �� ���� attack 1 ~5 ���� [1] ~ [5]�� �� ����
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("attack"))
            {
                if(clip.name[clip.name.Length - 1] - '0' < 6) // attack'1' '2' - '0' = 1, 2
                {
                    // Attack1  -> 1 -> (int)1
                    animLenths[clip.name[clip.name.Length - 1] - '0'] = clip.length;
                }
            }
        }
    }
    void GetInput()
    {
        /*
        if (gameManager == null)
        {
           // Debug.Log("���ӸŴ����� null �Դϴ�.");
            //return;
        }

        // ���߿� ���ӸŴ��� �����ϰ� ���� �̰ɷ� �ٲ�� ��
        //h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        //v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");
        */
        playerMove.h = curComboCount > defaultComboCnt ? 0 : Input.GetAxisRaw("Horizontal");
        playerMove.v = curComboCount > defaultComboCnt ? 0 : Input.GetAxisRaw("Vertical");

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
        if(isAttack) { playerMove.h = 0; playerMove.v = 0; return; }

        //animator.SetBool("isWolk", moveVec3 != Vector3.zero);
        //animator.SetBool("isRun", isRun);        
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

        animator?.SetBool("isAttack", isAttack);
        weapon?.UseEnd();

        GetSetting(weapon);

        playerAttack.weapon = this.weapon;
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
        if (Input.GetKeyDown(KeyCode.F) && CheckAttackDelay())
        {
            AttackExtention();
            //playerAttack.Attack();
        }
        
        // �޺� ���� �ð� �ʰ� �� �޺� �ʱ�ȭ
        // time.time, ������ ���۵� ������ ��ü �ð�(��)�� ��ȯ�ϴ� Unity���� �����ϴ� ����, �� ���� ������ ���۵Ǹ� 0���� �����Ͽ� ���������� ����
        // AttackAttempt���� lastAttackTime�� �������༭, 15��(���� �ð�) - 12��(������ �۵��� �ð�) > 0.3�� �̷������� �۵���
    }

    void AttackExtention()
    {
        //1-1) �÷��̾�� �޺� ī��Ʈ�� ����, �� ���� ��ư�� �޺�ī��Ʈ�� ���δ�
        curComboCount = (curComboCount % maxComboCount) + 1; //  1 ~ 5 1 2 3 4 0 -> 1
        animator.SetTrigger("Attack" + curComboCount.ToString());

        //1-2) ���� �ݶ��̴� Off -> On ���༭ Trigger �ٽ� �۵�
        weapon.Use(status.damage, curComboCount);
        
        // 1-3) ������ ���� �ð� ����
        lastAttackTime = Time.time;
        isAttack = true;
        isBaatle = true;

        Debug.Log($"{Time.time}: {curComboCount}");
    }

    bool CheckAttackDelay()
    {
        // ���� �� �� �ִ� ������ �ð��� �ƴ��� üũ�ϴ� �Լ�
        // animLenths [1] [2] [3] [4] [5] �� ����. [0] �� ��

        if (!isAttack) return true;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA" + curComboCount.ToString()) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f) return true;

        return false;

        //return (Time.time - lastAttackTime) > (attackDelay); // ����ð� - �������� ������ �ð� > ���� �ִϸ��̼��� ���� ��ġ�� ������ �� ����.
    }

    void CheckAttackEnd()
    {   // ������ ������ ������ �ϴ� �Լ�
        if (isAttack &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA" + curComboCount.ToString()) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
        {
            curComboCount = defaultComboCnt;
            weapon.UseEnd();
            isAttack = false;

            Debug.Log($"attack End: {animator.GetCurrentAnimatorStateInfo(0)} �۵�");
        }
    }

    void CheckTime()
    {
        CheckAttackDelay();
        CheckAttackEnd();
    }
    // ------------------------------------------------
    public void AddItem(string name, int num)
    {
        inventory.AddItem(name, num);
    }

    public int GetItemInInventory(ItemBase item)
    {
        return inventory.GetItemInInventory(item);
    }

    // -------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if(eDown && other.tag == "Pile")
        {
            other.GetComponent<IHarvestsAble>()?.Harvest();
            other.GetComponent<IPlantSeedAble>()?.PlantSeed(inventory.;
        }
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

            if (Input.GetButtonDown("Interaction")) animator.SetTrigger("Interaction");
        }
        else
            scanObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
            nearObject = null;
    }

    public void Damaged(float damage, Vector3 position)
    {
        Debug.Log("Player�� ���ݴ��ߴ�!");
        //status.hp -= Random.Range(status.armor - 3, status.armor + 1);
    }

    public Weapon GetWeapon() { return weapon; }
}
