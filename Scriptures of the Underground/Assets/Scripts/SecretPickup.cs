using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPickup : MonoBehaviour
{
    public InventorySecrets inventorysystem;
    public Item SecretObject;

    public PopUpUI popUi;

    // Start is called before the first frame update
    void Start()
    {
        popUi = GameObject.Find("TitleUi").GetComponent<PopUpUI>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerStats>().StunItemUp();
            popUi.StartFade(SecretObject.name, SecretObject.icon);
            AddItems();
        }
    }

    public void AddItems()
    {
        inventorysystem.Add(SecretObject);
        Destroy(gameObject);
    }
}
