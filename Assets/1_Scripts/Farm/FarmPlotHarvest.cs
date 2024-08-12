using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmPlotHarvest : MonoBehaviour, IHarvestsAble
{
    FarmPlot plot;
    Crops crop;
    public Item resultCropItem; // 수확할 아이템
    private bool isHarvestable = false; // 수확 가능 여부

    private void Awake()
    {
        plot = GetComponent<FarmPlot>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && plot.curFrits != null) // 마우스 왼쪽 클릭, 현재 작물이 있는지 체크
        {
            if (!EventSystem.current.IsPointerOverGameObject())     // UI 클릭 체크
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // RayCast
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    crop = plot.curFrits.GetComponent<Crops>();
                    if (hit.collider.gameObject == gameObject && crop.isComplete) // 이 오브젝트가 맞다면
                    {
                        Harvest();
                    }
                }
            }
        }
    }

    public void SetHarvestable(bool canHarvest)
    {
        isHarvestable = canHarvest;
    }

    public void Harvest()
    {
        // 수확 로직
        // 작물이 다 성장했는지 체크한다.
        // 마우스 클릭을 하면 성장 다 됐다면 수확을 시행한다
        // 수확을 하면 식물은 다시 init으로 setactvie false로 바꿔주고
        
        plot.currentSeed = null;
        plot.curFrits.SetActive(false);
        crop.isComplete = false;
        ItemManager.Instance.GiveToItem(crop.resultCropItemName, Random.Range(1, 5));
        crop = null;
    }
}
