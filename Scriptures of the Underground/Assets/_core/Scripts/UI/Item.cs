using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Secret", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    new public string name = "New item";
    public Sprite icon;
    public bool isDefaultItem = false;
    [TextArea]
    public string info;

    public virtual void Use()
    {
        //use the item
        //thing happens
        Debug.Log("Using" + name);
        InventorySecrets.instance.infoHolder.SetActive(true);
        InventorySecrets.instance.ChangeInfoText(info,icon);
    }
}
