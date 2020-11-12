using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class ThirdPersonController : MonoBehaviour
    {
        float horizontal;
        float vertical;
        Vector3 moveDirection;
        float moveAmount;
        Vector3 camYforward;

        public Transform camHolder;

        Rigidbody rigid;
        Collider col;
        Animator anim;

        public float moveSpeed = 4;
        public float sprintMultyplyer = 1.2f;
        public float rotSpeed = 9;
        public float jumpSpeed = 15;

        bool onGround;
        bool keepOffGround;
        bool climbOff;
        float climbTimer;
        float savedTime;

        bool mouseVisibleUnlocked = true;

        public bool isClimbing;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        Freeclimb freeClimb;

        public Camera cameraMain;
        private Vector2 rotation = Vector2.zero;
        public float lookSpeed = 3;
        bool croutching;

        [FMODUnity.EventRef]
        public string inputsound;
        FMOD.Studio.EventInstance footstepsEvent;
        public float inputSpeed;
        bool moving;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            col = GetComponent<Collider>();

            //camHolder = CameraHolder.singleton.transform;
            anim = GetComponentInChildren<Animator>();
            freeClimb = GetComponent<Freeclimb>();

            //identify gadget objects
            // gadgetMask = GameObject.Find("Gadget-Mask");

            //fmod stuff
            footstepsEvent = FMODUnity.RuntimeManager.CreateInstance(inputsound);


            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isClimbing)
            {
                return;
            }
            onGround = OnGround();
            Movement();
            LockAndHideCursorToggle();
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

        

        // Update is called once per frame
        void Update()
        {
            if (isClimbing)//disable and branch to free climb
            {
                freeClimb.Tick(Time.deltaTime);
                return;
            }

            onGround = OnGround();
            if (keepOffGround)
            {
                if(Time.realtimeSinceStartup - savedTime > 0.5)
                {
                    keepOffGround = false;
                }
            }

            Jump();

            if (Input.GetButtonDown("Crouch"))
            {
                Crouch();
            }

            if (Input.GetButtonDown("Interaction"))
            {
                Interact();
            }

            if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
            {
                moving = true;

            }
            else if(Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
            {
                moving = false;
            }

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



            anim.SetFloat("move", moveAmount);
            anim.SetBool("onAir", !onGround);
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

        bool OnGround()
        {
            if (keepOffGround)
                return false;
            

            if(Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
            {
                return true;
            }

            return false;
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
            }
            else
            {
                moveSpeed = (moveSpeed * 2);
                inputSpeed = (inputSpeed / 2);
                GetComponent<CapsuleCollider>().height = (GetComponent<CapsuleCollider>().height * 2);
                croutching = false;
            }
        }

        public void Interact()
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
        }

        public void Respawn()
        {
            transform.position = GetComponent<PlayerStats>().respawnLocation.transform.position;
        }

        private void OnDisable()
        {
            moving = false;
        }

        private void LockAndHideCursorToggle()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (mouseVisibleUnlocked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    mouseVisibleUnlocked = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    mouseVisibleUnlocked = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
    }
}

