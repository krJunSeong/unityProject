using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttackable
{
    int maxComboCount = 5;      // �ִ� �޺� ī��Ʈ
    int curComboCount = 0;      // ���� �޺�ī��Ʈ
    int defaultComboCnt = 0;
    private float lastAttackTime = 0f; // ������ ���� �ð�
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
        //1-1) �÷��̾�� �޺� ī��Ʈ�� ����, �� ���� ��ư�� �޺�ī��Ʈ�� ���δ�
        curComboCount = (curComboCount % maxComboCount) + 1; //  1 ~ 5 1 2 3 4 0 -> 1
        animator.SetTrigger("Attack" + curComboCount.ToString());

        //1-2) ���� �ݶ��̴� Off -> On ���༭ Trigger �ٽ� �۵�
        weapon.Use(player.status.damage, curComboCount);

        // 1-3) ������ ���� �ð� ����
        lastAttackTime = Time.time;
        isAttack = true;

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
}
