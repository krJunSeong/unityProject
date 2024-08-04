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

    [SerializeField] protected bool attackAble = false; // ���ݰ��� �����ΰ�?

    Dictionary<string, int> dicDamagedEneies = new Dictionary<string, int>(); // ���� ���� ����Ʈ Name : Ÿ��
    int[] comboHitCnts = new int[6];                                          // �� �޺��� �ִ� ���߼�

    protected int curCombo = 0;

    private void Awake()
    {
        InitSetting();
    }
    public virtual void Use(float dam, int curCombo)
    {
        bodyCollider.enabled = true;
        
        if(type == WeaponType.MELEE) damage = dam;

        if (attackAble)
        {
            this.curCombo = curCombo;
            UpdateDictionaryEnemies();
        }
    }
    protected virtual void InitSetting() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        bodyCollider = GetComponent<Collider>();

        // ����Ƽ�� �޺� ���� 1~5��Ÿ ����.
        comboHitCnts[1] = 1;
        comboHitCnts[2] = 1;
        comboHitCnts[3] = 1;
        comboHitCnts[4] = 1;
        comboHitCnts[5] = 3;
    }

    public virtual void UseEnd() 
    {
        if(bodyCollider != null) bodyCollider.enabled = false;
        this.curCombo = 0;

        if (attackAble) ClearDictionaryEnemies();
    }

    protected bool CheckDamagedList(string name)
    {
        // �������� �ǵ� ���� �ٴ���Ʈ üũ�ϴ� �Լ�. ������ ������ ���� true, �ƴ϶�� false ����.
        // �����ϰ� ���� ��ųʸ��� ���� �ֵ� �ִ� �Լ�
        // 1. WepaonTrigger�� ���� �浹�� ��ųʸ��� �ش� �̸��� �ִ��� üũ.
        // 2. ������ �̸� : Ÿ�� �߰�
        // 3. Ÿ���� 0 �̻��̸� -1 ���ְ� true ��ȯ
        // 4. ���� ������ ��ųʸ��� ���� �޺����� �°� Ÿ�� �߰�. = Use�Լ����� �� ��
        // 5. �޺��� �� �����ų� �������� �����ϸ� ��ųʸ� �����ڷ� ����. = UseEnd(), Ȥ�� �������� ���� üũ�˰��� �ʿ�
        // 6. OnTriggerEnter���� Damaged �۵��ϱ� ���ؼ� �� �Լ��� üũ�ϰ� �� ��.
        if (!attackAble) return false;

        //1.
        if(!dicDamagedEneies.ContainsKey(name))
        {
            //2.
            dicDamagedEneies[name] = comboHitCnts[curCombo];
        }

        //3.
        if (dicDamagedEneies[name] > 0)
        {
            dicDamagedEneies[name] -= 1;
            return true;
        }
        else
            return false;
    }

    void UpdateDictionaryEnemies()
    {
        List<string> keys = new List<string>(dicDamagedEneies.Keys);

        //4. ��ųʸ��� ���� �޺����� �°� Ÿ�� ����
        foreach(string i in keys)
        {
            dicDamagedEneies[i] = curCombo;
        }
    }

    void ClearDictionaryEnemies()
    {
        // 5.
        if(dicDamagedEneies.Count > 0) dicDamagedEneies.Clear();
    }

    public bool GetBodyCol() { return bodyCollider.enabled; }
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
