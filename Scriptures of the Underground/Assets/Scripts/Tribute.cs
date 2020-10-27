﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribute : MonoBehaviour
{
    bool inside;
    int uses = 1;

    public PlayerStats player;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("player is in the tribute zone");
            if (Input.GetButtonUp("Interaction") && uses >= 1)
            {
                player = other.GetComponent<PlayerStats>();
                Replenish(player);
                SetRespawn(player);
            }
            
        }
    }

    public void Replenish(PlayerStats _player)
    {
        Debug.Log("you've gaint stuff back and triggered a checkpoint");
        //gain a recource 
        player.GetComponent<PlayerStats>().StunItemUp();
        //save a checkpoint location
        uses--;
    }

    public void SetRespawn(PlayerStats _player)
    {
        _player.respawnLocation = transform.gameObject;
    }
}
