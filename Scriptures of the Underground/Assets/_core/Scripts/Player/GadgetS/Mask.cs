using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
    public bool masked;
    PlayerStats player;
    public Image maskImage;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerStatsHolder").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        maskImage.fillAmount = player.maskCharge / 100;
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
