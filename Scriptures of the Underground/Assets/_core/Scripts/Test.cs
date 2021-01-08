using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player IS COLLIDING!!!!");
        }
    }

    /*private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }*/
}
