using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    InventorySecrets inventory;

    InventorySlot[] slots;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventorySecrets.instance;
        inventory.OnItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void ToggleInventoryUi()
    {
        
        if (inventory.infoHolder.activeInHierarchy)
        {
            inventory.infoHolder.SetActive(false);
            anim.SetBool("journalActive", inventoryUI.activeSelf);
        }
        else
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            anim.SetBool("journalActive", inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        Debug.Log("Updating ui");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.secret.Count)
            {
                slots[i].AddItem(inventory.secret[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void CloseMainPage()
    {
        inventoryUI.SetActive(false);
    }
}
