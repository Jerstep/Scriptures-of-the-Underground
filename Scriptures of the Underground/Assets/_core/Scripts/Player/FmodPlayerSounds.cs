using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayerSounds : MonoBehaviour
{

    float distance = 0.1f;
    float Material;
    public LayerMask groundMask;
    public string jumpPath;

    public bool PlayerTouchingGround;
    bool PreviosulyTouchingGround;

    private void Update()
    {
        if (PlayerTouchingGround && Input.GetButtonDown("Jump"))
        {
            //CallJump();
        }
        if (!PreviosulyTouchingGround && PlayerTouchingGround)
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
            //PlayerTouchingGround = false;
        }
    }

    void CallFootsteps(string footstepsPath)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsPath);
        Footsteps.setParameterByName("Ground", Material);
        Footsteps.start();
        Footsteps.release();
    }

    void CallJump()
    {
        FMOD.Studio.EventInstance Jump = FMODUnity.RuntimeManager.CreateInstance(jumpPath);
        Jump.setParameterByName("Ground", Material);
        Jump.start();
        Jump.release();

    }
}
