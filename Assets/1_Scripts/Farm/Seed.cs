using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seed : ItemBase, IGrowAble
{
    SeedData data;
    public float growTime => data.growTime;

    private void Strat()
    {
        transform.localScale = data.initSscale;
    }
    public virtual void Grow() { transform.localScale += (Vector3.one * 0.1f);}
}
