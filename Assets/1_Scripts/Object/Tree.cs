using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ILoggingAble
{
    [SerializeField] float dropPersent;
    [SerializeField] float maxHp = 10.0f;
    [SerializeField] float hp = 10.0f;
    [SerializeField] float respawnTime = 3.0f;

    public void Cut(float dam)
    {
        hp -= dam;
        if (hp <= 0) Dead();
        if (Random.Range(0.0f, 100.0f) < dropPersent) DropItem();

        Debug.Log("Cut");
    }

    void Dead()
    {
        Invoke(nameof(Respawn), respawnTime);
        gameObject.SetActive(false);
    }

    void DropItem() 
    {
        Debug.Log("Item Drop");
    }

    public void Respawn()
    {
        hp = maxHp;
        gameObject.SetActive(true);
    }
}
