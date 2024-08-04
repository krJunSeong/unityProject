using UnityEngine;

public class FarmPlot : MonoBehaviour, IHarvestsAble, IPlantSeedAble
{
    private Seed currentSeed;
    private Transform seedPos;
    [SerializeField] GameObject[] fruits;

    private bool isGrowing;
    private float growTimer;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1)) PlantSeed(tempSeed.GetComponent<Seed>());

        if (isGrowing)
        {
            growTimer += Time.deltaTime;
            if(growTimer >= currentSeed.growTime / 10)
            {
                currentSeed.Grow();
                // �׷ο�Ÿ���� 1/10 ������ �Ǹ� ������ Up
                // currentSeed�� growTime�� �����ϸ� ��Ȯ �Ǵ� ��Ȯ���� ����
                growTimer = 0;
                if (currentSeed.transform.localScale.x >= 0.9)
                {
                    //Harvest();
                }
            }
        }
    }

    public void PlantSeed(Seed seed)
    {
        // ���� �ɴ� �ڵ�
        currentSeed = seed;
        growTimer = 0;
        isGrowing = true;

        // �Ʒ� �۹��� �� �̸��� �´� ���� ã�Ƽ� �ִ´�.
        foreach (var g in fruits)
        {
            if (g.name == seed.GetSeedData().fruitsName) 
            {
                g.gameObject.SetActive(true); 
                return; 
            }
        }
        // seedPos�� ������Ʈ�� �������ش�
        //Instantiate(seed.gameObject, transform.position, Quaternion.identity).transform.SetParent(seedPos);
    }

    public void Warter()
    {
        isGrowing = true;
    }
    public void Harvest()
    {
        isGrowing = false;
        currentSeed = null;

        // ��Ȯ ���� �߰� (�κ��丮�� ������ �߰� ��)
    }

    public bool IsEmpty() { return !isGrowing; }
}
