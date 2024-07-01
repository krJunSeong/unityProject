using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image background;
    public Image itemIcon;
    public Text itemCount;

    private Item currentItem;
    private int count;

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

    public bool IsSameItem(Item item)
    {
        return currentItem != null && currentItem.itemName == item.itemName;
    }

    public bool IsEmpty() { return currentItem == null? true : false; }

    public int GetCount() { return count; }
}
