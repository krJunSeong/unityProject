using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { MELEE, AXE, PIX }

    [SerializeField] protected WeaponType type;
    [SerializeField] protected float damage;
    [SerializeField] protected float delayTime;
    [SerializeField] protected int maxCombo;

    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Collider bodyCollider;

    [SerializeField] protected bool attackAble = false;

    protected int curCombo = 0;

    public virtual void Use(float dam)
    {
        bodyCollider.enabled = true;
        damage = dam;
    }
    protected virtual void InitSetting() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        bodyCollider = GetComponent<Collider>();
    }

    public virtual void UseEnd() 
    {
        if(bodyCollider != null) bodyCollider.enabled = false;
    }

    public float GetDelayTime() { return delayTime; }
    public int GetMaxCombo() { return maxCombo; }

    /* switch (type)
 {
     case WeaponType.MELEE:
         StopCoroutine("MeleeAttack");
         StartCoroutine("MeleeAttack");
         break;
     case WeaponType.AXE:
         break;
     case WeaponType.PIX:
         break;

 }*/

    /*
    IEnumerator MeleeAttack()
    {
        // �޺��� ����, ������ ����(������ �ݶ��̴��� ������ ��), ���� ��� ������ ����, �ڷ�ƾ���� �ۼ��� ��� �ڷ�ƾ ����� �޺� �ʱ�ȭ
        // update �Ἥ player���� �ش� Weapon�� GetComponent �ؼ� weapon setActive true false �ϴ� ������ ���� ���� ��õ
        // ���� isUsing ���� �� ������ �δ� ������ ������Ʈ ������ �� ������ üũ
        // �ڷ�ƾ���� ������ ��� while(true) { ���� } ���� stop��ų ��
        curCombo = ++curCombo % (limitCombo);
        isReady = false;
        // �����޺��� ����ϸ� isReady�� ������ �ʿ��ϴ�.
        // ����� �۵��� �� �� �� �ڿ� ���� ����� �� �� �־�� �Ѵ�
        // �ش� �޺��� �����޺���� ������ �ð� �ȿ��� ���Ⱑ �۵��ϸ� �� �ȴ�.
        yield return new WaitForSeconds(0.1f);

        isReady = false;
    }
    */
}
