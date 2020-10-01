using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerStunItem;
    public int keys;

    public GameplayUI UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StunItemUp()
    {
        playerStunItem++;
        UI.UpdateUIStun(playerStunItem);

    }
}
