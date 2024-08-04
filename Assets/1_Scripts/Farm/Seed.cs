using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemData     public string itemName;
//              public Sprite itmeImage;
//              public string itemDescription;
public class Seed : Item, IGrowAble
{
    [SerializeField] SeedData seedData;     // ¾¾¾Ñ µ¥ÀÌÅÍ
    public float growTime => seedData.growTime;

    //public Seed(SeedData data) : base(data) { seedData = data; }
    private void Strat()
    {
        base.itemData = seedData;
        transform.localScale = seedData.initSscale;
    }
    public virtual void Grow() { transform.localScale += (Vector3.one * 0.1f);}
    public SeedData GetSeedData() { return seedData; }
}
