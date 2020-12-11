using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class SecretPickup : MonoBehaviour
{
    public InventorySecrets inventorysystem;
    public Item SecretObject;

    public PopUpUI popUi;
    public GameplayUI UIScript;
    

    // Start is called before the first frame update
    void Start()
    {
        popUi = GameObject.Find("TitleUi").GetComponent<PopUpUI>();
        UIScript = GameObject.Find("GameplayUi").GetComponent<GameplayUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(UIScript.interactUI != UIScript.interactUI.activeInHierarchy)
            {
                UIScript.ChangeUIText(true);
                UIScript.interactUI.SetActive(true);
            }

            if (Input.GetButtonDown("Interaction"))
            {
                other.GetComponent<ThirdPersonController>().TakeNote();
                popUi.StartFade(SecretObject.name, SecretObject.icon);
                AddItems();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (UIScript.interactUI == UIScript.interactUI.activeInHierarchy)
            {
                UIScript.interactUI.SetActive(false);
            }
        }
    }

    public void AddItems()
    {
        UIScript.interactUI.SetActive(false);
        inventorysystem.Add(SecretObject);
        Destroy(gameObject);
        
    }
}
