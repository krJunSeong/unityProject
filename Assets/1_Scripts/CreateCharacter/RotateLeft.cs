using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : MonoBehaviour
{
    [SerializeField]
    Transform trp;

    [SerializeField]
    float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        trp.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
