using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    int collectiblesFound;
    int alertedEnemies;
    int deaths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    public void EndDemo(int collectibleAmount)
    {
        collectiblesFound = collectibleAmount;
    }
}
