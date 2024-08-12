using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemData     public string itemName;
//              public Sprite itmeImage;
//              public string itemDescription;
public class Seed : Item
{
    [SerializeField] SeedData seedData;     // ¾¾¾Ñ µ¥ÀÌÅÍ
    public float growTime => seedData.growTime;

    //public Seed(SeedData data) : base(data) { seedData = data; }
    private void Strat()
    {
        base.itemData = seedData;
        transform.localScale = seedData.initScale;
    }
    public SeedData GetSeedData() { return seedData; }
}
