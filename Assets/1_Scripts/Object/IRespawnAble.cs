using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnAble
{
    IEnumerator Respawn(float second);
}
