using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text headline;
    Item item;
    public Button button;

    public void AddItem(Item newItem)
    {
        item = newItem;
        headline.text = item.name;
        icon.sprite = item.icon;
        icon.enabled = true;
        button.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();

        }
    }
}