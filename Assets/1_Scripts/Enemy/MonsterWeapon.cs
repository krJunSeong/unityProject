using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    float damage;
    Collider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
        attackCollider.isTrigger = true;
    }

    public void Use(float useTime = 1.0f)
    {
        StopCoroutine(Attack(useTime));
        StartCoroutine(Attack(useTime));
    
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageAble>()?.Damaged(damage);
    }

    public void SetDamage(float dam) { damage = dam; }

    IEnumerator Attack(float useTime = 1.0f)
    {
        attackCollider.enabled = true;

        yield return new WaitForSeconds(useTime);

        attackCollider.enabled = false;
    }
}
