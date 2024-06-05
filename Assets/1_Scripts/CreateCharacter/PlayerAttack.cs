using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttackable
{
    int maxComboCount = 5;      // 최대 콤보 카운트
    int curComboCount = 0;      // 현재 콤보카운트
    int defaultComboCnt = 0;
    private float lastAttackTime = 0f; // 마지막 공격 시간
    public bool isAttack { get; set; }

    Animator animator;
    Player player;

    public Weapon weapon { get; set; }
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    void Start()
    {
        weapon = player.Weapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        //1-1) 플레이어는 콤보 카운트를 갖고, 매 공격 버튼시 콤보카운트를 높인다
        curComboCount = (curComboCount % maxComboCount) + 1; //  1 ~ 5 1 2 3 4 0 -> 1
        animator.SetTrigger("Attack" + curComboCount.ToString());

        //1-2) 무기 콜라이더 Off -> On 해줘서 Trigger 다시 작동
        weapon.Use(player.status.damage, curComboCount);

        // 1-3) 마지막 공격 시간 갱신
        lastAttackTime = Time.time;
        isAttack = true;

        Debug.Log($"{Time.time}: {curComboCount}");
    }
    bool CheckAttackDelay()
    {
        // 공격 할 수 있는 딜레이 시간이 됐는지 체크하는 함수
        // animLenths [1] [2] [3] [4] [5] 만 쓴다. [0] 안 씀

        if (!isAttack) return true;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA" + curComboCount.ToString()) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f) return true;

        return false;

        //return (Time.time - lastAttackTime) > (attackDelay); // 현재시간 - 마지막에 공격한 시간 > 현재 애니메이션의 일정 수치를 지났을 때 가능.
    }

    void CheckAttackEnd()
    {   // 공격이 끝나면 마무리 하는 함수
        if (isAttack &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA" + curComboCount.ToString()) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
        {
            curComboCount = defaultComboCnt;
            weapon.UseEnd();
            isAttack = false;

            Debug.Log($"attack End: {animator.GetCurrentAnimatorStateInfo(0)} 작동");
        }
    }

    void CheckTime()
    {
        CheckAttackDelay();
        CheckAttackEnd();
    }
}
