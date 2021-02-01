using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadNextLevel : MonoBehaviour
{
    public string levelToLoad;
    Gamemanager gman;

    // Start is called before the first frame update
    void Start()
    {
        gman = GameObject.Find("GameManager").GetComponent<Gamemanager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gman.EndDemo(other.GetComponentInChildren<PlayerStats>().storiesCollected);
        }
    }

   
}
