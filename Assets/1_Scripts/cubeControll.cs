using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cubeControll : MonoBehaviour
{
    // Start is called before the first frame update
    Transform obj;
    float angle;
    public Text text;
    float rotSpeed = 1.0f;

    void Start()
    {
        obj = GameObject.Find("SD_Unity-chan").transform;
    }

    void Update()
    {


        /*
            Vector3 vDirecterToTarget = (tfTarget.position - this.transform.position).normalized;
            vDirecterToTarget.y = 0f;

            // 3번째: 1239
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(vDirecterToTarget),
                                                  fRotSpeed * Time.deltaTime);
        */
        /*
        float Dot = Vector3.Dot(transform.forward.normalized, (obj.position - transform.position).normalized);

        float angle = Mathf.Acos(Dot) * Mathf.Rad2Deg;
        */
        float angle = Vector3.Angle(transform.forward, (obj.position - transform.position));

        text.text = angle.ToString();
        Debug.Log(angle);
    }
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    // y축만 돌리는 Rotate
    void RotateY()
    {
        // 바라는 결과값: Rotation.y 값만 돌아가는 것1
        // 1. Lookat으로 바라본다
        //2. 로테이션을 받는다
        //3. 로테이션의 x, z값을 0으로 고정시킨다.
        transform.LookAt(obj.transform);
        Vector3 v1 = transform.rotation.eulerAngles;
        v1.x = 0;
        v1.z = 0;
        transform.rotation = Quaternion.Euler(v1);
    }

    void GetAngle()
    {   // 앵글 구하는 곳
        //1. Dot(노말라이즈(F, obj.pos - pos))
        //2. Acos(dot) * Rag2Deg
        float Dot = Vector3.Dot(transform.forward.normalized, (obj.position - transform.position).normalized);

        float angle = Mathf.Acos(Dot) * Mathf.Rad2Deg;
    }
}
