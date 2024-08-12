using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour, IGrowAble
{
    // �۹��鿡 ���������� �� Ŭ����
    public bool isComplete;
    public string resultCropItemName; // �ڶ�� �۹� �̸�

    [SerializeField] float growTime; // �� ����, �� �ڶ�� �� �ɸ��� �ð�
    [SerializeField] Vector3 initScale = Vector3.one * 0.1f;
    
    float curTime; //
    float growPer; // 1/9�� ������ �� �� ��

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
            Debug.Log($"{curTime}, {gameObject.name} {growPer}�۵�");
            transform.localScale += (Vector3.one * 0.1f);

            yield return new WaitForSeconds(growPer);

            if (isComplete) break;
        }
    }
}
