using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour, IGrowAble
{
    // 작물들에 공통적으로 들어갈 클래스
    public bool isComplete;
    public string resultCropItemName; // 자라는 작물 이름

    [SerializeField] float growTime; // 초 단위, 다 자라는 데 걸리는 시간
    [SerializeField] Vector3 initScale = Vector3.one * 0.1f;
    
    float curTime; //
    float growPer; // 1/9로 성장할 때 쓸 것

    private void OnEnable()
    {
        transform.localScale = initScale;
    }

    void Update()
    {
        if (isComplete) return;

        curTime += Time.deltaTime;

        if(curTime > growTime)
        {
            isComplete = true;
            Debug.Log($"{gameObject.name} Complelete");
        }
    }

    public void StartGrow(Seed seed)
    {
        growTime = seed.GetSeedData().growTime;
        isComplete = false;
        growPer = growTime / 9;
        transform.localScale = seed.GetSeedData().initScale;
        resultCropItemName = seed.GetSeedData().harvestFruitItemName;
        StartCoroutine(Grow());
    }

    public IEnumerator Grow()
    {
        while (true)
        {
            Debug.Log($"{curTime}, {gameObject.name} {growPer}작동");
            transform.localScale += (Vector3.one * 0.1f);

            yield return new WaitForSeconds(growPer);

            if (isComplete) break;
        }
    }
}
