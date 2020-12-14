﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class ThirdPersonController : MonoBehaviour
    {

        float rayGroundDistance = 0.2f;
        float horizontal;
        float vertical;
        Vector3 moveDirection;
        float moveAmount;
        Vector3 camYforward;

        //character components
        Rigidbody rigid;
        Collider col;
        Animator anim;

        bool mouseVisibleUnlocked = false;

        //camera position
        public Transform camHolder;

        //speeds of the character
        public float moveSpeed = 4;
        float ogMoveSpeed;
        public float sprintMultyplyer = 1.2f;
        public float rotSpeed = 9;
        public float jumpSpeed = 15;


        //ground checks and climbing checks
        bool onGround;
        bool keepOffGround;
        bool climbOff;
        float climbTimer;
        float savedTime;
        public bool isClimbing;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        //scripts we refrence
        public PlayerStats playerstats;
        Freeclimb freeClimb;

        public Camera cameraMain;
        private Vector2 rotation = Vector2.zero;
        public float lookSpeed = 3;
        bool croutching;

        //Fmod stuffs
        [FMODUnity.EventRef]
        public string inputsound;
        FMOD.Studio.EventInstance footstepsEvent;
        public float inputSpeed;
        bool moving;
        public bool canMove;

        // Start is called before the first frame update
        void Start()
        {
            ogMoveSpeed = moveSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            //setting the difrent components
            col = GetComponent<Collider>();
            anim = GetComponentInChildren<Animator>();
            freeClimb = GetComponent<Freeclimb>();

            //fmod creating the instance for sound
            footstepsEvent = FMODUnity.RuntimeManager.CreateInstance(inputsound);


            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //if its climbing don't execute movement or check functions
            if (isClimbing)
            {
                return;
            }

            GroundCheck();

            Movement();
    
        } 

        

        // Update is called once per frame
        void Update()
        {
            //lock the cursor or not
            LockAndHideCursorToggle();

            if (isClimbing)//disable and branch to free climb
            {
                freeClimb.Tick(Time.deltaTime);
                return;
            }
            if (keepOffGround)
            {
                if(Time.realtimeSinceStartup - savedTime > 0.5)
                {
                    keepOffGround = false;
                }
            }
            //jump trigger
            Jump();


            if (Input.GetButtonDown("Crouch"))
            {
                Crouch();
            }



            if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
            {
                moving = true;

            }
            else if(Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
            {
                moving = false;
            }

            //climbing checks 
            if (!onGround && !keepOffGround)
            {
                if (!climbOff)
                {
                    isClimbing = freeClimb.CheckForClimb();
                    if (isClimbing)
                    {
                        DisableController();
                    }
                }   
            }

            if (climbOff)
            {
                if (Time.realtimeSinceStartup - climbTimer > 1)
                {
                    climbOff = false;
                }
            }

            //set animations parameters
            anim.SetFloat("move", moveAmount);
            anim.SetBool("onAir", !onGround);
        }

        void Movement()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            // Look();
            camYforward = camHolder.forward;
            Vector3 v = vertical * camHolder.forward;
            Vector3 h = horizontal * camHolder.right;

            moveDirection = (v + h).normalized;
            moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)));

            if (!canMove)
            {
                moveSpeed = 0;
            }
            else
            {
                moveSpeed = ogMoveSpeed;
            }

            Vector3 targetDir = moveDirection;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * rotSpeed);
            transform.rotation = targetRot;

            Vector3 dir = transform.forward * (Input.GetButton("Sprint") ? (moveSpeed * sprintMultyplyer) * moveAmount : moveSpeed * moveAmount);
            dir.y = rigid.velocity.y;
            rigid.velocity = dir;
        }


        void Jump()
        {
            if (onGround)
            {
                bool jump = Input.GetButton("Jump");
                if (jump)
                {
                    Vector3 v = rigid.velocity;
                    v.y = jumpSpeed;
                    rigid.velocity = v;
                    savedTime = Time.realtimeSinceStartup;
                    keepOffGround = true;
                }
            }
        }


        public void GroundCheck()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.down * rayGroundDistance, Color.green);
            Physics.Raycast(groundCheck.position, Vector3.down, out hit, rayGroundDistance, groundMask);
            if (hit.collider)
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }
        }

        public void DisableController()
        {
            rigid.isKinematic = true;
            col.enabled = false;
        }

        public void EnableController()
        { 
            rigid.isKinematic = false;
            col.enabled = true;
            anim.CrossFade("onAir", 0.2f);
            climbOff = true;
            climbTimer = Time.realtimeSinceStartup;
            isClimbing = false;
        }

        public void Crouch()
        {
            Debug.Log("crouch bro,or not");
            if (!croutching)
            {
                moveSpeed = (moveSpeed / 2);
                inputSpeed = (inputSpeed * 2);
                GetComponent<CapsuleCollider>().height = (GetComponent<CapsuleCollider>().height / 2);
                croutching = true;
                anim.SetBool("Crouch", croutching);
            }
            else
            {
                moveSpeed = (moveSpeed * 2);
                inputSpeed = (inputSpeed / 2);
                GetComponent<CapsuleCollider>().height = (GetComponent<CapsuleCollider>().height * 2);
                croutching = false;
                anim.SetBool("Crouch", croutching);
            }
        }

       /* public void Interact()
        {
            Vector3 origin = transform.position;
            origin.y += 0.4f;
            Vector3 direction = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, 3f))
            {
                if (hit.transform.tag == "InteractableButton")
                {
                    hit.transform.gameObject.GetComponent<OpenerButtonController>().openDoor();
                    Debug.Log("yo broham show ham we interacted you see that shiii");
                }
            }
        }*/

        public void Respawn()
        {
            transform.position = playerstats.respawnLocation.transform.position;
        }


        public void TakeNote()
        {
            anim.SetBool("takingNote", true);
        }

        private void OnDisable()
        {
            moving = false;
        }

        private void LockAndHideCursorToggle()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                Debug.Log("inventory pressed");
                if (mouseVisibleUnlocked)
                {
                    Debug.Log("inventory pressed mouseVisibleUnlocked");
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    mouseVisibleUnlocked = false;
                }
                else
                {
                    Debug.Log("inventory pressed NOT mouseVisibleUnlocked");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    mouseVisibleUnlocked = true;
                }
            }
        }

        public void MoveToggle()
        {
            canMove = !canMove;
            moving = !moving;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                Respawn();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            //Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
    }
}

