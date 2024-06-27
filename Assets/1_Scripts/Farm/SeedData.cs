using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Seed", menuName = "SeedData", order = 52)]
public class SeedData : ScriptableObject
{
    public string seedName;
    public float growTime; // √ 
    public Vector3 initSscale = Vector3.one * 0.1f;
    public Vector3 finalScale = Vector3.one;
    public Sprite seedImage;
}
