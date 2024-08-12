using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrowAble
{
    public void StartGrow(Seed seed);
    public IEnumerator Grow();
}
