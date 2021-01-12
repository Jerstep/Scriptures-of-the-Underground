using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayerSounds : MonoBehaviour
{

    float distance = 0.1f;
    float Material;
    public LayerMask groundMask;
    public string jumpPath,journalOpenPath, journalClosePath, journalPageflipPath, sketchPath,gadgetSwitchPath, gadetFirePath;

    public bool PlayerTouchingGround;
    bool PreviosulyTouchingGround;

    private void Update()
    {//calling the sound
        if (PlayerTouchingGround && Input.GetButtonDown("Jump"))
        {
            //CallJump();
        }
        else if (!PreviosulyTouchingGround && PlayerTouchingGround)
        {
            //CallJump();
        }
        PreviosulyTouchingGround = PlayerTouchingGround;
    }

    private void FixedUpdate()
    {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.blue);
    }

    //checking where we stand/land on to play the right sound
    void MaterialCheck()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, distance, groundMask);
        if (hit.collider)
        {
            PlayerTouchingGround = true;
            if (hit.collider.tag == "Material:Dirt")
            {
                Material = 1f;
            }
            else
            {
                Material = 0f;
            }
        }
        else
        {
            PlayerTouchingGround = false;
        }
    }

    //playing footstep sounds
    void CallFootsteps(string footstepsPath)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsPath);
        Footsteps.setParameterByName("Ground", Material);
        Footsteps.start();
        Footsteps.release();
    }

    //playing jumping sounds
    void CallJump()
    {
        FMOD.Studio.EventInstance Jump = FMODUnity.RuntimeManager.CreateInstance(jumpPath);
        Jump.setParameterByName("Ground", Material);
        Jump.start();
        Jump.release();

    }

    void CallJournal()
    {
        Debug.Log("HALO JOURNAL SOUND");
        FMOD.Studio.EventInstance JournalOpen = FMODUnity.RuntimeManager.CreateInstance(journalOpenPath);
        JournalOpen.start();
        JournalOpen.release();
    }

    public void CallJournalClose()
    {
        FMOD.Studio.EventInstance JournalClose = FMODUnity.RuntimeManager.CreateInstance(journalClosePath);
        JournalClose.start();
        JournalClose.release();
    }

    public void CallJournalPageflip()
    {
        FMOD.Studio.EventInstance JournalPageflip = FMODUnity.RuntimeManager.CreateInstance(journalPageflipPath);
        JournalPageflip.start();
        JournalPageflip.release();
    }

    public void CallSkech()
    {
        FMOD.Studio.EventInstance SketchPath = FMODUnity.RuntimeManager.CreateInstance(sketchPath);
        SketchPath.start();
        SketchPath.release();
    }

    public void CallGadgetSwitch()
    {
        FMOD.Studio.EventInstance GadgetSwitch = FMODUnity.RuntimeManager.CreateInstance(gadgetSwitchPath);
        GadgetSwitch.start();
        GadgetSwitch.release();
    }

    public void CallFire()
    {
        FMOD.Studio.EventInstance GadetFirePath = FMODUnity.RuntimeManager.CreateInstance(gadetFirePath);
        GadetFirePath.start();
        GadetFirePath.release();
    }
}
