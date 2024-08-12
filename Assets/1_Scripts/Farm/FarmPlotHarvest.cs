using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmPlotHarvest : MonoBehaviour, IHarvestsAble
{
    FarmPlot plot;
    Crops crop;
    public Item resultCropItem; // ��Ȯ�� ������
    private bool isHarvestable = false; // ��Ȯ ���� ����

    private void Awake()
    {
        plot = GetComponent<FarmPlot>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && plot.curFrits != null) // ���콺 ���� Ŭ��, ���� �۹��� �ִ��� üũ
        {
            if (!EventSystem.current.IsPointerOverGameObject())     // UI Ŭ�� üũ
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // RayCast
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    crop = plot.curFrits.GetComponent<Crops>();
                    if (hit.collider.gameObject == gameObject && crop.isComplete) // �� ������Ʈ�� �´ٸ�
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
        // ��Ȯ ����
        // �۹��� �� �����ߴ��� üũ�Ѵ�.
        // ���콺 Ŭ���� �ϸ� ���� �� �ƴٸ� ��Ȯ�� �����Ѵ�
        // ��Ȯ�� �ϸ� �Ĺ��� �ٽ� init���� setactvie false�� �ٲ��ְ�
        
        plot.currentSeed = null;
        plot.curFrits.SetActive(false);
        crop.isComplete = false;
        ItemManager.Instance.GiveToItem(crop.resultCropItemName, Random.Range(1, 5));
        crop = null;
    }
}
