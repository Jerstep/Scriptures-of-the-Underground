using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySecrets : MonoBehaviour
{
   
    #region Singleton
    public static InventorySecrets instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public bool gotGirl;
    public int space = 20;
    public List<Item> secret = new List<Item>();

    public GameObject infoHolder;
    public GameObject infoTextBox;

    private void Start()
    {
        //infoHolder = GameObject.Find("information-Holder");
        //infoTextBox = GameObject.Find("information-Text");
    }

    public bool Add(Item painting)
    {
        if (!painting.isDefaultItem)
        {
            if (secret.Count >= space)
            {
                Debug.Log("No Space in your Inventory");
                return false;
            }

            secret.Add(painting);
            if (OnItemChangedCallback != null)
                OnItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(Item painting)
    {
        secret.Remove(painting);
        if (OnItemChangedCallback != null)
            OnItemChangedCallback.Invoke();
    }

    public void ChangeInfoText(string info)
    {
        infoTextBox.GetComponent<Text>().text = info;
    }
}