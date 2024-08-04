using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image background;
    public Image itemIcon;
    public Text itemCount;

    [SerializeField] protected Item currentItem;
    protected int count;

    public bool isSpread = false; // 오브젝트가 비활성화 되면 표시할 건가요?

    private void Awake()
    {
        Debug.Log("Awake called for " + gameObject.name);

        background = GetComponent<Image>();
        itemCount = GetComponentInChildren<Text>(true);
        itemIcon = GetComponentsInChildren<Image>(true)[1];
    }

    private void OnEnable()
    {
        CheckSpreadSlot();
    }
    public void SetItem(Item item, int count)
    {
        currentItem = item;
        this.count = count;

        itemIcon.sprite = item.icon;
        itemIcon.gameObject.SetActive(true);
        itemCount.text = count.ToString();
        itemCount.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        currentItem = null;
        count = 0;

        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
        itemCount.text = "";
        itemCount.gameObject.SetActive(false);
    }
    public void AddItemCount(int amount)
    {
        count += amount;
        itemCount.text = count.ToString();
    }
    public void DecreaceItemCnt(int amount)
    {
        count -= amount;
        if (count < 1)
        {
            currentItem = null;
            itemIcon = null;
        }

        itemCount.text = amount.ToString();
    }
    public bool IsSameItem(Item item)
    {
        return currentItem != null && currentItem.itemName == item.itemName;
    }
    public bool IsEmpty() { return currentItem == null ? true : false; }
    public int GetCount() { return count; }
    public Item GetItem() { return currentItem; }
    public void CheckSpreadSlot()
    {
        // 객체 비활성화 표시 true, 현재 아이템이 없을 경우 슬롯을 안 보이게 하는 함수.
        if (currentItem == null && isSpread) gameObject.SetActive(false);
    }
    public void SetItemData(Item item, int cnt)
    {
        // 농장용 슬롯 세팅
        currentItem = item;
        itemIcon.sprite = (item == null? null : item.itemData.itemImage);
        count = cnt;
        itemCount.text = "0";
    }
}
