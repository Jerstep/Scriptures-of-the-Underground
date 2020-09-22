using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPickup : MonoBehaviour
{
    public InventorySecrets inventorysystem;
    public Item SecretObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AddItems();
        }
    }

    public void AddItems()
    {
        inventorysystem.Add(SecretObject);
        Destroy(gameObject);
    }
}
