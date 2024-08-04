using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Seed", menuName = "Item/SeedData", order = 52)]
public class SeedData : ItemData
{
    public float growTime; // 초 단위, 다 자라는 데 걸리는 시간
    public Vector3 initSscale = Vector3.one * 0.1f;
    public Vector3 finalScale = Vector3.one;
    public string fruitsName; // 자라는 작물 이름
}
