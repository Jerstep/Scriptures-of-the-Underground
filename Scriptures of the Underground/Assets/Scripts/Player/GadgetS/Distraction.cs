using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    PlayerStats player;
    public GameObject bulletPrefab;
    

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }


    private void OnEnable()
    {
        player.AimCamTurnOn();
    }

    void OnDisable()
    {
        player.AimCamTurnOff();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interaction") && player.bullets >= 1)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        //instantiate bullet based on this objects location and based on the amount of bullets player has left
        //Instantiate(bulletPrefab, gameObject.transform.position);
    }
}
