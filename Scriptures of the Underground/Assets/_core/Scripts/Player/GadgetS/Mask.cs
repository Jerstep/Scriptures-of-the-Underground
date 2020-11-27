using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public bool masked;
    PlayerStats player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerStatsHolder").GetComponent<PlayerStats>();
    }

    //turning the mask on and off
    private void OnEnable()
    {
        if(player.maskCharge != 0)
        {
            masked = true;
            player.MaskedFunction(masked);
        }
        
    }

    void OnDisable()
    {
        masked = false;
        player.MaskedFunction(masked);
    }

}
