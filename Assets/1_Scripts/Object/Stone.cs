using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, ILoggingAble
{
    [SerializeField] ObjectState state;
    [SerializeField] string dropItemName = "Stone";


    void Awake()
    {
        state.Init(100.0f, 10.0f, 10.0f, 3.0f);
    }
    public void Used(float dam)
    {
        state.hp -= dam;
        if (state.hp <= 0) Dead(); 
    }

    void Dead()
    {
        if (Random.Range(0.0f, 100.0f) < state.dropPersent) DropItem(Random.Range(1, 4));

        Invoke(nameof(Respawn), state.respawnTime);
        gameObject.SetActive(false);
    }

    void DropItem(int num)
    {
        GameManager.Instance.GetItemEffect(dropItemName, this.transform.position, num);   // 게임매니저 이펙트 발생.  게임매니저에서 아이템 Give 함수 작동
    }

    public void Respawn()
    {
        state.hp = state.maxHp;
        gameObject.SetActive(true);
    }
}
