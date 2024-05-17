using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMonterTrace : MonoBehaviour, IDamageAble
{
    // 간단한 쫓아가면서 공격하는 몬스터 클래스

    [SerializeField] float hp = 100.0f;             // 체력
    [SerializeField] float armor = 2.0f;             // 체력

    [SerializeField] int   damage = 10;             // 공격력
    [SerializeField] float attackRange = 1.5f;      // 공격 범위
    [SerializeField] int   attackMaxCombo = 2;      // 공격콤보수
    [SerializeField] float limitAttackDelay = 2.0f; // 공격 딜레이 리미트 시간
    [SerializeField] float limitComboTime = 1.0f;   // 콤보 공격 기다려주는 시간
    int curCombo = 0;                               // 현재 콤보수
    float lastAttackTime = 0.0f;                    // 마지막 공격 시간
    
    bool isAttackAble;

    [SerializeField] float detectionRange = 10f; // 플레이어 감지 범위
    [SerializeField] float stoppingDistance = 3f; // 플레이어와의 정지 거리
    [SerializeField] float detectingTime = 0.5f;  // 플레이어 정보 갱신 주기
    [SerializeField] float speed = 2.0f;          // 이동속도
    [SerializeField] float distance;                               // 대상과의 거리

    private NavMeshAgent navMeshAgent;  // 네비게이션 에이전트
    private Animator animator;          // 애니메이터 컴포넌트
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
         // 플레이어 찾기
        InvokeRepeating("CheckPlayerDistance", 0f, detectingTime); // 0.5초마다 플레이어 감지
    }

    private void CheckPlayerDistance()
    {
        // 플레이어와의 거리 측정
        distance = Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPosition());

        // 플레이어 감지 범위 내에 있으면 추적 시작
        if ((distance < detectionRange) && (distance > stoppingDistance))
        { 
            navMeshAgent.SetDestination(GameManager.Instance.GetPlayerPosition()); // 플레이어를 향해 이동
            animator.SetBool("IsMoving", true); // 이동 애니메이션 재생
        }
        else
        {   
            navMeshAgent.ResetPath();           // 이동 중인 경로 초기화
            animator.SetBool("IsMoving", false); // 이동 애니메이션 정지
        }

    }

    private void Update()
    {
        CheckAttackAble();

        if(distance < detectionRange) RotateTowards(GameManager.Instance.GetPlayerPosition());
        // 플레이어와의 거리가 공격 범위 내에 있으면 공격
        if ((distance < attackRange) && isAttackAble)
        {
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }   

    // 플레이어에게 공격을 가하는 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 줌
            collision.gameObject.GetComponent<IDamageAble>()?.Damaged(damage);
        }
    }

    void CheckAttackAble()
    {
        if (!isAttackAble) lastAttackTime += Time.deltaTime;

        // 1. 콤보 시간을 지나면 콤보를 초기화한다.
        if (limitComboTime <= lastAttackTime) ResetAttack();

        // 2. 어택 딜레이 시간이 지나면 어택할 수 있게 한다.
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
        // 현재 객체의 위치와 목표 위치를 이용해 방향 벡터 계산
        Vector3 direction = target - transform.position;

        // 방향 벡터의 y값을 0으로 설정하여 y축 회전만 고려
        direction.y = 0;

        // 방향 벡터가 (0,0,0)이면 회전하지 않음
        if (direction == Vector3.zero) return;

        // 목표 방향으로의 회전을 계산
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 현재 회전에서 목표 회전까지 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f); // 5.0f는 회전 속도 조절용
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
