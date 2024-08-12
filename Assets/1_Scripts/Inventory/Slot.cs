using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Text itemCount;
    [SerializeField] private Image background;
    [SerializeField] private Image itemIcon;
    [SerializeField] protected Item currentItem;

    private Image originItemIcon;
    private Sprite originItemIconSprite;
    protected int count;

    public bool isSpread = false; // 오브젝트가 비활성화 되면 표시할 건가요?

    private void Awake()
    {
        background = GetComponent<Image>();
        itemCount = GetComponentInChildren<Text>(true);
        itemIcon = GetComponentsInChildren<Image>(true)[1];
        originItemIcon = itemIcon;
        originItemIconSprite = itemIcon.sprite;
    }

    private void OnEnable()
    {
        CheckSpreadSlot();
    }
    public void SetItem(Item item, int count)
    {
        currentItem = item;
        this.count = count;

        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.icon;
        itemCount.gameObject.SetActive(true);
        itemCount.text = count.ToString();
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
        itemIcon.gameObject.SetActive(true);
        itemCount.gameObject.SetActive(true);
        count += amount;
        itemCount.text = count.ToString();
    }
    public void DecreaceItemCnt(int amount)
    {
        count -= amount;
        if (count < 1)
        {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
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
