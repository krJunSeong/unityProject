using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image background;
    public Image itemIcon;
    public Text itemCount;

    [SerializeField] protected Item currentItem;
    protected int count;

    public bool isSpread = false; // ������Ʈ�� ��Ȱ��ȭ �Ǹ� ǥ���� �ǰ���?

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
        // ��ü ��Ȱ��ȭ ǥ�� true, ���� �������� ���� ��� ������ �� ���̰� �ϴ� �Լ�.
        if (currentItem == null && isSpread) gameObject.SetActive(false);
    }
    public void SetItemData(Item item, int cnt)
    {
        // ����� ���� ����
        currentItem = item;
        itemIcon.sprite = (item == null? null : item.itemData.itemImage);
        count = cnt;
        itemCount.text = "0";
    }
}
