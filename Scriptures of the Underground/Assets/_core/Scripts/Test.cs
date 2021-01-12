using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string audiopath;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player IS COLLIDING!!!!");
            CallAudio();
        }
    }

    public void CallAudio()
    {
        FMOD.Studio.EventInstance AudioPath = FMODUnity.RuntimeManager.CreateInstance(audiopath);
        AudioPath.start();
        AudioPath.release();
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
