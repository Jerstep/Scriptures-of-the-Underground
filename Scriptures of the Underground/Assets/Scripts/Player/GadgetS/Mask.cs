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
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        masked = true;
        player.MaskedFunction(masked);
    }

    void OnDisable()
    {
        masked = false;
        player.MaskedFunction(masked);
    }

}
