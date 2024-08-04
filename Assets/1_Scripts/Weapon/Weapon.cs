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

    [SerializeField] protected bool attackAble = false; // 공격가능 무기인가?

    Dictionary<string, int> dicDamagedEneies = new Dictionary<string, int>(); // 맞은 대상들 리스트 Name : 타수
    int[] comboHitCnts = new int[6];                                          // 각 콤보별 최대 적중수

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

        // 유니티쟝 콤보 공격 1~5연타 기초.
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
        // 개발자의 의도 외의 다단히트 체크하는 함수. 공격이 가능한 얘라면 true, 아니라면 false 리턴.
        // 공격하고 무기 딕셔너리에 맞은 애들 넣는 함수
        // 1. WepaonTrigger로 적과 충돌시 딕셔너리에 해당 이름이 있는지 체크.
        // 2. 없으면 이름 : 타수 추가
        // 3. 타수가 0 이상이면 -1 해주고 true 반환
        // 4. 어택 때마다 딕셔너리에 현재 콤보수에 맞게 타수 추가. = Use함수에서 쓸 것
        // 5. 콤보가 다 끝나거나 전투상태 종료하면 딕셔너리 내부자료 삭제. = UseEnd(), 혹은 전투상태 종료 체크알고리즘 필요
        // 6. OnTriggerEnter에서 Damaged 작동하기 위해선 이 함수로 체크하고 쓸 것.
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

        //4. 딕셔너리에 현재 콤보수에 맞게 타수 수정
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
        // 콤보수 증가, 데미지 로직(무기의 콜라이더와 연결할 것), 무기 사용 딜레이 로직, 코루틴으로 작성할 경우 코루틴 종료시 콤보 초기화
        // update 써서 player에서 해당 Weapon을 GetComponent 해서 weapon setActive true false 하는 식으로 쓰는 것을 추천
        // 변수 isUsing 같은 걸 웨폰에 두는 것으로 업데이트 돌릴지 안 돌리지 체크
        // 코루틴으로 구현할 경우 while(true) { 로직 } 으로 stop시킬 것
        curCombo = ++curCombo % (limitCombo);
        isReady = false;
        // 연속콤보를 사용하면 isReady에 조건이 필요하다.
        // 모션이 작동한 뒤 몇 초 뒤에 다음 모션을 쓸 수 있어야 한다
        // 해당 콤보가 최종콤보라면 딜레이 시간 안에는 무기가 작동하면 안 된다.
        yield return new WaitForSeconds(0.1f);

        isReady = false;
    }
    */
}
