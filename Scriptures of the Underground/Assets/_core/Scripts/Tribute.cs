using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribute : MonoBehaviour
{
    bool inside;
    int uses = 1;
    public bool firstTribute;

    public PlayerStats player;
    GameplayUI UIScript;
    SphereCollider spherecol;

    void Start()
    {
        UIScript = GameObject.Find("GameplayUi").GetComponent<GameplayUI>();
        spherecol = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (UIScript.interactUI != UIScript.interactUI.activeInHierarchy)
            {
                UIScript.interactUI.SetActive(true);
            }

            Debug.Log("player is in the tribute zone");
            if (Input.GetButtonUp("Interaction") && uses >= 1)
            {
                player = other.GetComponentInChildren<PlayerStats>();
                Replenish(player);
                SetRespawn(player);
                spherecol.enabled = false;
                UIScript.interactUI.SetActive(false);
                if (firstTribute)
                {
                    Getmask();
                }
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

    public void Replenish(PlayerStats _player)
    {
        Debug.Log("you've gaint stuff back and triggered a checkpoint");
        //gain a recource 
        player.GetComponent<PlayerStats>().BulletsItemUp();
        //save a checkpoint location
        uses--;
    }

    public void SetRespawn(PlayerStats _player)
    {
        _player.respawnLocation = transform.gameObject;
    }

    public void Getmask()
    {
        UIScript.SetMask();
    }
}
