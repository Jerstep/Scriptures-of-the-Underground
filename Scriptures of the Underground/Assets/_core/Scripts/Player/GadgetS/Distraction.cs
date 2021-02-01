using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    PlayerStats player;
    FmodPlayerSounds playerSounds;
    public GameObject bulletPrefab;

    public float cooldowntimer = 1;
    public float currentCooldown = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerStatsHolder").GetComponent<PlayerStats>();
        playerSounds = GameObject.Find("PlayerCharacter_V2").GetComponent<FmodPlayerSounds>();
    }


    private void OnEnable()
    {
        //player.AimCamTurnOn();
    }

    void OnDisable()
    {
        //player.AimCamTurnOff();
    }

    private void Update()
    {
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else if (currentCooldown < 0)
        {
            currentCooldown = 0;
        }
       

        if (Input.GetAxis("Shoot") == 1 && currentCooldown == 0 && player.bullets >= 1 || Input.GetButtonDown("Shoot2") && currentCooldown == 0 && player.bullets >= 1)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        //instantiate bullet based on this objects location and based on the amount of bullets player has left
        //Instantiate(bulletPrefab, gameObject.transform.position);
        Debug.Log("shoot");
        playerSounds.CallFire();
        currentCooldown = cooldowntimer;
        player.bullets--;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        
    }
}
