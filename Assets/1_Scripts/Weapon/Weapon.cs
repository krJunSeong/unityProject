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
