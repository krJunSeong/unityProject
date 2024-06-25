using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMonterTrace : MonoBehaviour, IDamageAble
{
    // 간단한 쫓아가면서 공격하는 몬스터 클래스

    /*사용법
     기본 세팅으로 무기에 콜라이더를 세팅하고, MonsterWeapon 컴포넌트를 부착해야 합니다.
    애니메이터의 공격 애니메이션은 Attack1 Attack2 이런 식으로 명명해야 합니다.
    Idle <-> attack1 애니메이션 조건은 attack1 true, false로 합니다.
     */
    [SerializeField] float hp = 100.0f;             // 체력
    [SerializeField] float maxHp = 100.0f;          // 체력
    [SerializeField] float armor = 2.0f;            // 아머
    [SerializeField] int   damage = 10;             // 공격력
    [SerializeField] float attackRange = 1.5f;      // 공격 범위
    [SerializeField] int   attackMaxCombo = 2;      // 최대 콤보수
    [SerializeField] float limitAttackDelay = 2.0f; // 공격 딜레이 리미트 시간
    
    [SerializeField] bool isConnectCombo2 = false;  // 2번째 공격까지 바로 할 것인가 설정
    [SerializeField] bool isConnectCombo3 = false;  // 3번째 공격까지 바로 할 것인가 설정
    [SerializeField] float detectionRange = 10f;    // 플레이어 감지 범위
    [SerializeField] float stoppingDistance = 3f;   // 플레이어와의 정지 거리
    [SerializeField] float detectingTime = 0.5f;    // 플레이어 정보 갱신 주기
    [SerializeField] float speed = 2.0f;            // 이동속도
    [SerializeField] float distanceToPlayer;        // 대상과의 거리
    [SerializeField] float isSturnMotionTime = 0.8f;// 스턴 모션 시간

    int damagedCnt = 0;
    int curCombo = 0;                               // 현재 콤보수
    float lastAttackTime = 0.0f;                    // 마지막 공격 시간
    private float testCnt = 0;

    bool isAttackAble;                          // 공격가능 유무
    bool isDead = false;                        // 죽음 유무
    bool isSturn = false;                       // 스턴 유무
    bool isAttacking = false;                   // 공격 중 유무

    private NavMeshAgent navMeshAgent;  // 네비게이션 에이전트
    private Animator animator;          // 애니메이터 컴포넌트
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
         // 플레이어 찾기
        InvokeRepeating("CheckPlayerDistance", 0f, detectingTime); // 0.5초마다 플레이어 감지
    }

    private void CheckPlayerDistance()
    { 
        // 플레이어와의 거리 측정
            
        distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPosition());

        // 플레이어 감지 범위 내에 있으면 추적 시작
        if ((distanceToPlayer < detectionRange) && (distanceToPlayer > stoppingDistance) && !isAttacking)
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
        /*  구현 순서
           1. 공격 콤보 구현
                1.DelayTime을 준다
                2. Attack 1 2 Idle 순서로 돌아온다
                3. AttackAble, 애니메이션 체크
           2. isSturn 당할 경우, 모션 시간 동안, 공격 이동 못함, 스턴 모션 실행
                                공격 초기화(lastAttak = time.time;), 콤보 0으로 초기화, 이동 못함(isStopped)
           3. 죽음 << 작업중 <<
                1. 죽을 경우 공격 이동 등 아무것도 못함
                2. 사라질 때 천천히 사라질 것
                3. bodyCollider 끌 것.

            4. 되살리기(풀링 대비)
                1. maxHP 둘 것
                
           추가사항 
                1. 공격시 안 움직이게 할 것
                2. 콤보공격을 동시에 진행할 것  
        
        구현
            1. Attack 1을 하고 만약 isConnectComboAttack2 = true라면, 1을 하고 바로 2까지 공격한다.
            2. bool isAttacking이 true라면, animator.Setbool(ismoving , false),

        문제 Player의 여러 타수 문제.
            - 원하는 기능: 한 콤보에는 1타, 2타, 3타, 4타, 5타
            - 구현1
                - 필요한 것들: 이름: 현재 타수
                -             이름: 콤보 타수
                -             [] 각 콤보별 타수
                - 1. OnTriggerEnter == (tag == monster)?
                    - 1. 딕셔너리에 이름이 있는지 체크
                    - 2. 없으면 등록
                    - 3. 딕셔너리로 이름 : (int)타수를 저장한다.
                - 2. <이름: 타수>에 각 콤
                - 3. 현재 콤보 == 타수, 1콤보는 안 맞고 2콤보에 맞았을 경우도 생각.
                - 4. 그럼 몇 타수를 맞았는지 체크는 어디서 하는가?

            - 구현2
                - 수동으로 각 애니메이션의 끝 조사해서 끌 타이밍을 배열로 만들 것
                - 
         */

        if (Input.GetKeyUp(KeyCode.F1)) { StartCoroutine(Dead()); Invoke("ReLife", 3.0f); };
        if (Input.GetKeyUp(KeyCode.F2)) { StartCoroutine(Sturn(isSturnMotionTime)); };

        CheckMoving();          // 특정 설정 동안에는 이동 막는 함수
        CheckAttackAble();      // 공격 가능하도록 체크하는 함수 
        if (distanceToPlayer < detectionRange) RotateTowards(GameManager.Instance.GetPlayerPosition()); // player를 향해 rotate하는 함수

        if (distanceToPlayer <= attackRange && isAttackAble) Attack();        
        CheckAttackAnimation(); // 공격 애니메이션 체크
    }
    void CheckMoving()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            if (isAttacking || isSturn || isDead) { navMeshAgent.isStopped = true; }
            else { navMeshAgent.isStopped = false; }
        }
    }
    void CheckAttackAble()
    {
        // 2. 어택 딜레이 시간이 지나면 어택할 수 있게 한다.
        if (Time.time - lastAttackTime >= limitAttackDelay) isAttackAble = true;
    }

    void CheckAttackAnimation()
    {
        if (isSturn) return;
        // 어택 애니메이션이 70% 진행됐으면 해당 애니메이션을 false로 바꾸는 함수
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
        // 공격하는 함수
        isAttackAble = false;
        isAttacking = true;

        curCombo++;
        curCombo = curCombo > attackMaxCombo ? 1 : Mathf.Clamp(curCombo, 1, attackMaxCombo);

        lastAttackTime = Time.time;

        // 어택하면 이전 어택 단계의 파라미터 false로 하고 true로 하는 부분
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

    public void Damaged(float damage, Vector3 position)
    {
        float _damage = damage - armor > 0 ? damage - armor : 0;

        FontManager.Instance.ShowDamage(damage, position);
        hp -= _damage;
        Debug.Log($"test: {++testCnt}, Hp: {hp}");
        if (hp <= 0.0f)
        {
            StopCoroutine(Dead());
            StartCoroutine(Dead());
        }
        else
        {
            // 만약 스턴을 여러번 당한다면? 맞을 때마다 억~ 억~ 하면서 스턴 걸리는 거지 그런 경우에는?
            StopCoroutine(Sturn(isSturnMotionTime));
            StartCoroutine(Sturn(isSturnMotionTime));
        }
    }

    public void AnimDead()
    {
        // Animation에서 쓸 함수
        isDead = true;
        animator.SetTrigger("Die");
        this.gameObject.SetActive(false);
    }

    IEnumerator Sturn(float time)
    {
        //4. isSturn 당할 경우, 모션 시간 동안, 공격 이동 못함, 스턴 모션 실행
        //                      공격 초기화(lastAttak = time.time;), 콤보 0으로 초기화, 이동 못함(isStopped)
        animator.SetTrigger("TrSturn");
        lastAttackTime = Time.time;
        curCombo = 0;
        isSturn = true;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isSturn = false;
    }

    IEnumerator Dead()
    {
        /*  3.죽음
                1.죽을 경우 공격 이동 등 아무것도 못함 = CheckMoving, Attack, CheckAnimation
                2.사라질 때 천천히 사라질 것 = Dead(), 알파값 처리
                3.bodyCollider 끌 것. = Dead()
                4.animation motion    = Dead()
                5. 애니메이션 trDead, trSturn 생성
        */
        DeadSetting();
        CancelInvoke("CheckPlayerDistance");

        Color color;
        while (true)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == null) continue;

                color = materials[i].color;
                color.a = Mathf.Lerp(1f, 0f, animator.GetCurrentAnimatorStateInfo(0).normalizedTime); // 애니메이션 Die에 맞춰서 setactive를 한 박자 늦게 꺼야한다.
                materials[i].color = color;

                yield return new WaitForSeconds(0.1f);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f) { gameObject.SetActive(false); break; }
        }
    }

    void DeadSetting()
    {
        isDead = true;
        isAttackAble = false;
        isAttacking = false;
        isSturn = false;
        animator.SetTrigger("TrDead");
        bodyCollider.enabled = false;
    }

    void ReLife()
    {
        Debug.Log("ReLife 작동");
        isDead = false;
        hp = maxHp;        
        isAttackAble = true;
        isAttacking = false;
        isSturn = false;
        bodyCollider.enabled = true;

        Color color;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] == null) continue;

            color = materials[i].color;
            color.a = 1; // 애니메이션 Die에 맞춰서 setactive를 한 박자 늦게 꺼야한다.
            materials[i].color = color;
        }

        gameObject.SetActive(true);
        InvokeRepeating("CheckPlayerDistance", 0f, detectingTime);
    }
}