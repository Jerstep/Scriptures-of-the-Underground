using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //playing footstep sounds
    public void CallSound(string SoundPath)
    {
        Debug.Log("HALO "+ SoundPath + " SOUND");
        FMOD.Studio.EventInstance Sound = FMODUnity.RuntimeManager.CreateInstance(SoundPath);
        Sound.start();
        Sound.release();
    }
}
