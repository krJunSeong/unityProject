using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMonterTrace : MonoBehaviour, IDamageAble
{
    // ������ �Ѿư��鼭 �����ϴ� ���� Ŭ����

    /*����
     �⺻ �������� ���⿡ �ݶ��̴��� �����ϰ�, MonsterWeapon ������Ʈ�� �����ؾ� �մϴ�.
    �ִϸ������� ���� �ִϸ��̼��� Attack1 Attack2 �̷� ������ ����ؾ� �մϴ�.
    Idle <-> attack1 �ִϸ��̼� ������ attack1 true, false�� �մϴ�.
     */
    [SerializeField] float hp = 100.0f;             // ü��
    [SerializeField] float maxHp = 100.0f;             // ü��
    [SerializeField] float armor = 2.0f;            // �Ƹ�
    [SerializeField] int   damage = 10;             // ���ݷ�
    [SerializeField] float attackRange = 1.5f;      // ���� ����
    [SerializeField] int   attackMaxCombo = 2;      // �ִ� �޺���
    [SerializeField] float limitAttackDelay = 2.0f; // ���� ������ ����Ʈ �ð�
    [SerializeField] float limitComboTime = 1.0f;   // �޺� ���� ��ٷ��ִ� �ð�
    
    [SerializeField] bool isConnectCombo2 = false;  // 2��° ���ݱ��� �ٷ� �� ���ΰ� ����
    [SerializeField] bool isConnectCombo3 = false;  // 3��° ���ݱ��� �ٷ� �� ���ΰ� ����
    [SerializeField] float detectionRange = 10f;    // �÷��̾� ���� ����
    [SerializeField] float stoppingDistance = 3f;   // �÷��̾���� ���� �Ÿ�
    [SerializeField] float detectingTime = 0.5f;    // �÷��̾� ���� ���� �ֱ�
    [SerializeField] float speed = 2.0f;            // �̵��ӵ�
    [SerializeField] float defaultSpeed = 2.0f;     // �⺻�ӵ�
    [SerializeField] float distanceToPlayer;        // ������ �Ÿ�
    [SerializeField] float deadMotionTime = 1.0f;   // ���� ��� �ð�
    [SerializeField] float isSturnMotionTime = 0.8f;// ���� ��� �ð�

    int damagedCnt = 0;
    int curCombo = 0;                               // ���� �޺���
    float lastAttackTime = 0.0f;                    // ������ ���� �ð�
    
    bool isAttackAble;                          // ���ݰ��� ����
    bool isDead = false;                        // ���� ����
    bool isSturn = false;                       // ���� ����
    bool isAttacking = false;                   // ���� �� ����

    private NavMeshAgent navMeshAgent;  // �׺���̼� ������Ʈ
    private Animator animator;          // �ִϸ����� ������Ʈ
    private MonsterWeapon monsterWeapon;
    private Collider bodyCollider;
    [SerializeField] Material[] materials = new Material[5];

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterWeapon = GetComponentInChildren<MonsterWeapon>();
        navMeshAgent.speed = speed > 0? speed : navMeshAgent.speed;
        GetComponentInChildren<MonsterWeapon>()?.SetDamage(damage);
        bodyCollider = GetComponent<Collider>();
        Renderer[] r = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < r.Length; i++)
        {
            materials[i] = r[i].material;
        }
    }

    private void Start()
    {
         // �÷��̾� ã��
        InvokeRepeating("CheckPlayerDistance", 0f, detectingTime); // 0.5�ʸ��� �÷��̾� ����
    }

    private void CheckPlayerDistance()
    { 
        // �÷��̾���� �Ÿ� ����
        distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPosition());

        // �÷��̾� ���� ���� ���� ������ ���� ����
        if ((distanceToPlayer < detectionRange) && (distanceToPlayer > stoppingDistance) && !isAttacking)
        { 
            navMeshAgent.SetDestination(GameManager.Instance.GetPlayerPosition()); // �÷��̾ ���� �̵�
            animator.SetBool("IsMoving", true); // �̵� �ִϸ��̼� ���
        }
        else
        {   
            navMeshAgent.ResetPath();           // �̵� ���� ��� �ʱ�ȭ
            animator.SetBool("IsMoving", false); // �̵� �ִϸ��̼� ����
        }
    }

    private void Update()
    {
        /*  ���� ����
           1. ���� �޺� ����
                1.DelayTime�� �ش�
                2. Attack 1 2 Idle ������ ���ƿ´�
                3. AttackAble, �ִϸ��̼� üũ
           2. isSturn ���� ���, ��� �ð� ����, ���� �̵� ����, ���� ��� ����
                                ���� �ʱ�ȭ(lastAttak = time.time;), �޺� 0���� �ʱ�ȭ, �̵� ����(isStopped)
           3. ���� << �۾��� <<
                1. ���� ��� ���� �̵� �� �ƹ��͵� ����
                2. ����� �� õõ�� ����� ��
                3. bodyCollider �� ��.

            4. �ǻ츮��(Ǯ�� ���)
                1. maxHP �� ��
                
           �߰����� 
                1. ���ݽ� �� �����̰� �� ��
                2. �޺������� ���ÿ� ������ ��  
        
        ����
            1. Attack 1�� �ϰ� ���� isConnectComboAttack2 = true���, 1�� �ϰ� �ٷ� 2���� �����Ѵ�.
            2. bool isAttacking�� true���, animator.Setbool(ismoving , false),
         */

        if (Input.GetKeyUp(KeyCode.F1)) StartCoroutine(Dead());
        CheckMoving();          // Ư�� ���� ���ȿ��� �̵� ���� �Լ�
        CheckAttackAble();      // ���� �����ϵ��� üũ�ϴ� �Լ� 
        if (distanceToPlayer < detectionRange) RotateTowards(GameManager.Instance.GetPlayerPosition()); // player�� ���� rotate�ϴ� �Լ�

        if (distanceToPlayer <= attackRange && isAttackAble) Attack();        
        CheckAttackAnimation(); // ���� �ִϸ��̼� üũ
    }
    void CheckMoving()
    {
        if (isAttacking || isSturn || isDead) { navMeshAgent.isStopped = true; }
        else { navMeshAgent.isStopped = false; }
    }
    void CheckAttackAble()
    {
        // 2. ���� ������ �ð��� ������ ������ �� �ְ� �Ѵ�.
        if (Time.time - lastAttackTime >= limitAttackDelay) isAttackAble = true;
    }

    void CheckAttackAnimation()
    {
        if (isSturn) return;
        // ���� �ִϸ��̼��� 70% ��������� �ش� �ִϸ��̼��� false�� �ٲٴ� �Լ�
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            if (isConnectCombo2) { Attack();}
            else isAttacking = false;            
            animator.SetBool("Attack1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if (isConnectCombo3) { Attack();}
            else isAttacking = false;
            animator.SetBool("Attack2", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            animator.SetBool("Attack3", false);
            isAttacking = false;
        }
    }

    void Attack()
    {
        if (isSturn) return;
        // �����ϴ� �Լ�
        isAttackAble = false;
        isAttacking = true;

        curCombo++;
        curCombo = curCombo > attackMaxCombo ? 1 : Mathf.Clamp(curCombo, 1, attackMaxCombo);

        lastAttackTime = Time.time;

        // �����ϸ� ���� ���� �ܰ��� �Ķ���� false�� �ϰ� true�� �ϴ� �κ�
        switch (curCombo)
        {
            case 1:
                animator.SetBool("Attack1", true);
                break;

            case 2:
                animator.SetBool("Attack2", true);
                animator.SetBool("Attack1", false);
                break;

            case 3:
                animator.SetBool("Attack3", true);
                animator.SetBool("Attack2", false);
                break;
        }

        monsterWeapon.Use(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void RotateTowards(Vector3 target)
    {
        // ���� ��ü�� ��ġ�� ��ǥ ��ġ�� �̿��� ���� ���� ���
        Vector3 direction = target - transform.position;

        // ���� ������ y���� 0���� �����Ͽ� y�� ȸ���� ���
        direction.y = 0;

        // ���� ���Ͱ� (0,0,0)�̸� ȸ������ ����
        if (direction == Vector3.zero) return;

        // ��ǥ ���������� ȸ���� ���
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ���� ȸ������ ��ǥ ȸ������ ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f); // 5.0f�� ȸ�� �ӵ� ������
    }

    public void Damaged(float damage)
    {
        hp -= Random.Range(armor - 2, armor + 1);

        Debug.Log($"���ݴ��ߴ� ���� {++damagedCnt}");
        if (hp <= 0.0f)
        {
            StopCoroutine(Dead());
            StartCoroutine(Dead());
        }
        else
        {
            // ���� ������ ������ ���Ѵٸ�? ���� ������ ��~ ��~ �ϸ鼭 ���� �ɸ��� ���� �׷� ��쿡��?
            StopCoroutine(Sturn(isSturnMotionTime));
            StartCoroutine(Sturn(isSturnMotionTime));
        }
    }

    public void AnimDead()
    {
        // Animation���� �� �Լ�
        isDead = true;
        animator.SetTrigger("Die");
        this.gameObject.SetActive(false);
    }

    IEnumerator Sturn(float time)
    {
        //4. isSturn ���� ���, ��� �ð� ����, ���� �̵� ����, ���� ��� ����
        //                      ���� �ʱ�ȭ(lastAttak = time.time;), �޺� 0���� �ʱ�ȭ, �̵� ����(isStopped)
        animator.SetTrigger("TrSturn");
        lastAttackTime = Time.time;
        curCombo = 0;
        isSturn = true;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isSturn = false;
    }

    IEnumerator Dead()
    {
        /*  3.����
                1.���� ��� ���� �̵� �� �ƹ��͵� ���� = CheckMoving, Attack, CheckAnimation
                2.����� �� õõ�� ����� �� = Dead(), ���İ� ó��
                3.bodyCollider �� ��. = Dead()
                4.animation motion    = Dead()
                5. �ִϸ��̼� trDead, trSturn ����
        */

        isDead = true;
        isAttackAble = false;
        isAttacking = true;
        isSturn = false;
        animator.SetTrigger("TrDead");
        bodyCollider.enabled = false;

        CancelInvoke("CheckPlayerDistance");

        Color color;
        while (true)
        {
            for(int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == null) continue;

                color = materials[i].color;
                color.a /= animator.GetCurrentAnimatorStateInfo(0).length; // �ִϸ��̼� Die�� ���缭 setactive�� �� ���� �ʰ� �����Ѵ�.
                materials[i].color = color;

            }
            if (materials[0].color.a <= 0.1f) break;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);
        }

        gameObject.SetActive(false);

    }
}
