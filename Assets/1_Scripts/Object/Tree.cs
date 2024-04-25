using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ILoggingAble
{
    [SerializeField] ObjectState state;

    private void Awake()
    {
        state.Init(100.0f, 10.0f, 10.0f, 3.0f);
    }

    public void Used(float dam)
    {
        state.hp -= dam;
        if (state.hp <= 0) Dead();

        Debug.Log("Cut");
    }

    void Dead()
    {
        if (Random.Range(0.0f, 100.0f) < state.dropPersent) DropItem();
        Invoke(nameof(Respawn), state.respawnTime);
        gameObject.SetActive(false);
    }

    void DropItem() 
    {
        Debug.Log("Item Drop");
    }

    public void Respawn()
    {
        state.hp = state.maxHp;
        gameObject.SetActive(true);
    }
}
