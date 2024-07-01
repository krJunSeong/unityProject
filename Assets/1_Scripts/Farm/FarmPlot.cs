using UnityEngine;

public class FarmPlot : MonoBehaviour, IHarvestsAble, IPlantSeedAble
{
    private Seed currentSeed;
    private Transform seedPos;
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
                // 그로우타임의 1/10 수준이 되면 스케일 Up
                // currentSeed의 growTime에 도달하면 수확 또는 수확가능 상태
                growTimer = 0;
                if (currentSeed.transform.localScale.x >= 0.9)
                {
                    Harvest();
                }
            }
        }
    }

    public void PlantSeed(Seed seed)
    {
        // 씨앗 심는 코드
        currentSeed = seed;
        growTimer = 0;

        // seedPos에 오브젝트를 생성해준다
        //Instantiate(seed.gameObject, transform.position, Quaternion.identity).transform.SetParent(seedPos);
    }

    public void Warter()
    {
        isGrowing = true;
    }
    public void Harvest()
    {
        isGrowing = false;
        Destroy(seedPos.gameObject);
        currentSeed = null;

        // 수확 로직 추가 (인벤토리에 아이템 추가 등)
    }

}
