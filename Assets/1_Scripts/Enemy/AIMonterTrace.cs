using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMonterTrace : MonoBehaviour, IDamageAble
{
    // ������ �Ѿư��鼭 �����ϴ� ���� Ŭ����

    [SerializeField] float hp = 100.0f;             // ü��
    [SerializeField] float armor = 2.0f;             // ü��

    [SerializeField] int   damage = 10;             // ���ݷ�
    [SerializeField] float attackRange = 1.5f;      // ���� ����
    [SerializeField] int   attackMaxCombo = 2;      // �����޺���
    [SerializeField] float limitAttackDelay = 2.0f; // ���� ������ ����Ʈ �ð�
    [SerializeField] float limitComboTime = 1.0f;   // �޺� ���� ��ٷ��ִ� �ð�
    int curCombo = 0;                               // ���� �޺���
    float lastAttackTime = 0.0f;                    // ������ ���� �ð�
    
    bool isAttackAble;

    [SerializeField] float detectionRange = 10f; // �÷��̾� ���� ����
    [SerializeField] float stoppingDistance = 3f; // �÷��̾���� ���� �Ÿ�
    [SerializeField] float detectingTime = 0.5f;  // �÷��̾� ���� ���� �ֱ�
    [SerializeField] float speed = 2.0f;          // �̵��ӵ�
    [SerializeField] float distance;                               // ������ �Ÿ�

    private NavMeshAgent navMeshAgent;  // �׺���̼� ������Ʈ
    private Animator animator;          // �ִϸ����� ������Ʈ
    private MonsterWeapon monsterWeapon;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterWeapon = GetComponentInChildren<MonsterWeapon>();
        navMeshAgent.speed = speed > 0? speed : navMeshAgent.speed;
        GetComponentInChildren<MonsterWeapon>()?.SetDamage(damage);
    }

    private void Start()
    {
         // �÷��̾� ã��
        InvokeRepeating("CheckPlayerDistance", 0f, detectingTime); // 0.5�ʸ��� �÷��̾� ����
    }

    private void CheckPlayerDistance()
    {
        // �÷��̾���� �Ÿ� ����
        distance = Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPosition());

        // �÷��̾� ���� ���� ���� ������ ���� ����
        if ((distance < detectionRange) && (distance > stoppingDistance))
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
        CheckAttackAble();

        if(distance < detectionRange) RotateTowards(GameManager.Instance.GetPlayerPosition());
        // �÷��̾���� �Ÿ��� ���� ���� ���� ������ ����
        if ((distance < attackRange) && isAttackAble)
        {
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }   

    // �÷��̾�� ������ ���ϴ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �������� ��
            collision.gameObject.GetComponent<IDamageAble>()?.Damaged(damage);
        }
    }

    void CheckAttackAble()
    {
        if (!isAttackAble) lastAttackTime += Time.deltaTime;

        // 1. �޺� �ð��� ������ �޺��� �ʱ�ȭ�Ѵ�.
        if (limitComboTime <= lastAttackTime) ResetAttack();

        // 2. ���� ������ �ð��� ������ ������ �� �ְ� �Ѵ�.
        if (limitAttackDelay <= lastAttackTime) isAttackAble = true;
    }
    void Attack()
    {
        monsterWeapon.Use(0.8f);
        isAttackAble = false;
        curCombo = (curCombo % attackMaxCombo) + 1 ;  // 0 ~ 1 -> 1 ~ 2
        lastAttackTime = 0;
        animator.SetTrigger("Attack" + curCombo.ToString());
    }

    void ResetAttack()
    {
        curCombo = 0;
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
       hp -= Random.Range(armor - 3, armor + 1);

       if (hp <= 0.0f) Dead();
    }

    void Dead()
    {

    }
}
