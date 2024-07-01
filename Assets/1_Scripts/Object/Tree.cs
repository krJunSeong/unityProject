using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ILoggingAble
{
    [SerializeField] ObjectState state;
    [SerializeField] string dropItemName = "Tree";

    private void Awake()
    {
        state.Init(100.0f, 10.0f, 10.0f, 3.0f);
    }

    public void Used(float dam)
    {
        state.hp -= dam;
        Debug.Log($"dam: {dam}");
        if (state.hp <= 0) Dead();
    }

    void Dead()
    {
        //if (Random.Range(0.0f, 100.0f) < state.dropPersent) DropItem();
        Invoke(nameof(Respawn), state.respawnTime);
        DropItem(Random.Range(1, 4));
        gameObject.SetActive(false);
    }

    void DropItem(int num) 
    {
        // 아이템 드랍하는 함수
        GameManager.Instance.GetItemEffect(dropItemName, this.transform.position, num);   // 게임매니저 이펙트 발생.  게임매니저에서 아이템 Give 함수 작동
    }

    public void Respawn()
    {
        state.hp = state.maxHp;
        gameObject.SetActive(true);
    }
}
