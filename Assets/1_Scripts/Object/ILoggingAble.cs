using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct ObjectState
{
    public float dropPersent;
    public float maxHp;
    public float hp;
    public float respawnTime;

    public void Init(float dropPer, float maxHp, float hp, float respawnTime)
    {
        dropPersent = dropPer;
        this.maxHp = maxHp;
        this.hp = hp;
        this.respawnTime = respawnTime;
    }
}

public interface ILoggingAble
{
    void Used(float dam);
}
