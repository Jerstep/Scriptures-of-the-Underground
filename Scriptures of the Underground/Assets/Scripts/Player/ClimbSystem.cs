using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : MonoBehaviour
{
    public float characterWith = 0.35f;
    public float climbForce;
    public float smallestEdge = 0.25f;
    public float coolDown = 0.15f;
    public float climbRange = 2;
    //30 tends to be for regulair people 45 for trained ones anything else probably superheroes
    public float maxAngle = 30;
    public float jumpForce = 1;

    public Climbingsort currentSort;
    public Transform handTrans;
    public Animator animator;
    public float minDistance;
    public Rigidbody rigid;
    public CharacterController tpuc;
    public PlayerMovement playerMove;
    public Vector3 verticalHandOffset;
    public Vector3 horizontalHandOffset;
    public Vector3 fallHandOffset;
    public Vector3 raycastPos;

    public LayerMask spotLayer;
    public LayerMask currentSpotLayer;
    public LayerMask checkLayersForObstacle;
    public LayerMask checkLayersReachable;

    private Vector3 targetPoint;
    private Vector3 targetNormal;

    private float lastTime;
    private float beginDistance;
    private RaycastHit hit;
    private Quaternion oldRotation;


    // Update is called once per frame
    void Update()
    {
        
        if (currentSort == Climbingsort.Walking && Input.GetAxis("Vertical") > 0)
        {
            StartClimbing();
        }

        if(currentSort == Climbingsort.Climbing)
        {
            Climb();
        }

        UpdateStats();

        if(currentSort == Climbingsort.ClimbingTowardsPoint || currentSort == Climbingsort.ClimbingTowardsPlateau)
        {
            MoveTowardsPoint();
        }

        if(currentSort == Climbingsort.Jumping || currentSort == Climbingsort.Falling)
        {
            print(" we jumping");
            Jumping();
        }
    }

    public void UpdateStats()
    {
        if(currentSort != Climbingsort.Walking && playerMove.isGrounded && currentSort != Climbingsort.ClimbingTowardsPoint)
        {
            currentSort = Climbingsort.Walking;
            playerMove.enabled = true;
            rigid.isKinematic = false;
        }

        if(currentSort == Climbingsort.Walking && !playerMove.isGrounded)
        {
            currentSort = Climbingsort.Jumping;
        }

        if(currentSort == Climbingsort.Walking && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            CheckForClimbStart();
        }

    }

    public void StartClimbing()
    {
        
        if(Physics.Raycast(transform.position + transform.rotation * raycastPos, transform.forward, 0.4f) && Time.time - lastTime > coolDown && currentSort == Climbingsort.Walking)
        {//he says sometimes it adds double force thats why he checks 2 times but might be bull so may remove second walking check
            if(currentSort == Climbingsort.Walking)
            {
                rigid.AddForce(transform.up * jumpForce);

            }

            lastTime = Time.time;
        }
    }

    public void Jumping()
    {
        print(" we jumping function");
        if (rigid.velocity.y < 0 && currentSort != Climbingsort.Falling)
        {
            currentSort = Climbingsort.Falling;
            oldRotation = transform.rotation;

        }

        if(rigid.velocity.y > 0 && currentSort != Climbingsort.Jumping)
        {
            currentSort = Climbingsort.Jumping;
        }

        if(currentSort == Climbingsort.Jumping)
        {
            CheckForSpots(handTrans.position + fallHandOffset, -transform.up, 0.1f,CheckingSort.Normal);
            
        }
        if(currentSort == Climbingsort.Falling)
        {//                                                                              if you miss to much change the value   HERE
            CheckForSpots(handTrans.position + fallHandOffset + transform.rotation * new Vector3(0.2f, -0.6f, 0), -transform.up, 0.4f, CheckingSort.Normal);
            transform.rotation = oldRotation;
        }
    }

    public void Climb()
    {
        //if you climbing and you passed cooldown period
        if(Time.time - lastTime > coolDown && currentSort == Climbingsort.Climbing)
        {
            if(Input.GetAxis("Vertical") > 0)
            {
                //upclimb
                CheckForSpots(handTrans.position + transform.rotation * verticalHandOffset + transform.up * climbRange, -transform.up, climbRange,CheckingSort.Normal);
                
                if(currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    CheckForPlateau();
                }
               
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                //downclimb
                CheckForSpots(handTrans.position - transform.rotation * ( verticalHandOffset + new Vector3(0,0.3f,0)), -transform.up, climbRange, CheckingSort.Normal);

                if(currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    rigid.isKinematic = false;
                    playerMove.enabled = true;
                    currentSort = Climbingsort.Falling;
                    oldRotation = transform.rotation;

                }
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                //left right climb
                CheckForSpots(handTrans.position + transform.rotation * horizontalHandOffset , transform.right * Input.GetAxis("Horizontal") - transform.up /3.5f, climbRange, CheckingSort.Normal);

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    CheckForSpots(handTrans.position + transform.rotation * horizontalHandOffset, transform.right * Input.GetAxis("Horizontal") - transform.up / 1.5f, climbRange/3, CheckingSort.Normal);
                }
                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    CheckForSpots(handTrans.position + transform.rotation * horizontalHandOffset, transform.right * Input.GetAxis("Horizontal") - transform.up / 6, climbRange / 1.5f, CheckingSort.Normal);
                }
                //turning
                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    int hor = 0;
                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        hor = -1;
                    }
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        hor = 1;
                    }

                    CheckForSpots(handTrans.position + transform.rotation * horizontalHandOffset + transform.right * hor * smallestEdge/4,transform.forward - transform.up * 2, climbRange / 3, CheckingSort.Turning);

                    if (currentSort != Climbingsort.ClimbingTowardsPoint)
                    {//debug.drawrecast to find best spot 
                        CheckForSpots(handTrans.position + transform.rotation * horizontalHandOffset + transform.right * 0.2f, transform.forward - transform.up * 2 + transform.right * hor / 1.5f, climbRange / 3, CheckingSort.Turning);
                    }

                }
            }

        }
    }

    public void CheckForSpots(Vector3 spotLocation,Vector3 dir,float range,CheckingSort sort)
    {
        print(" checking spot");
        bool foundSpot = false;
        Debug.DrawRay(handTrans.position, dir * range, Color.green);
        if (Physics.Raycast(spotLocation - transform.right * smallestEdge/2, dir,out hit,range,spotLayer))
        {
           
            print(hit.transform.name);
            if (Vector3.Distance(handTrans.position, hit.point) > minDistance)
            {
                foundSpot = true;
                //
                FindSpot(hit, sort);
                
            }
        }

        if (!foundSpot)
        {
            if (Physics.Raycast(spotLocation + transform.right * smallestEdge / 2, dir, out hit, range, spotLayer))
            {
                if (Vector3.Distance(handTrans.position , hit.point) - smallestEdge / 1.5f > minDistance)
                {
                    foundSpot = true;
                    //
                    FindSpot(hit, sort);
                }
            }
        }

        if (!foundSpot)
        {
            if (Physics.Raycast(spotLocation + transform.right * smallestEdge / 2 + transform.forward * smallestEdge, dir, out hit, range, spotLayer))
            {
                if (Vector3.Distance(handTrans.position, hit.point) - smallestEdge / 1.5f > minDistance)
                {
                    foundSpot = true;
                    //
                    FindSpot(hit, sort);
                }
            }
        }

        if (!foundSpot)
        {
            if (Physics.Raycast(spotLocation - transform.right * smallestEdge / 2 + transform.forward * smallestEdge, dir, out hit, range, spotLayer))
            {
                if (Vector3.Distance(handTrans.position, hit.point) > minDistance)
                {
                    foundSpot = true;
                    //
                    FindSpot(hit,sort);
                }
            }
        }
    }

    public void FindSpot(RaycastHit h, CheckingSort sort)
    {
        if(Vector3.Angle(h.normal,Vector3.up) < maxAngle)
        {
            RayInfo ray = new RayInfo();

            print("found spot");
            if(sort == CheckingSort.Normal)
            {
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0, 0.01f, 0), transform.forward/2.5f);
            }
            else if (sort == CheckingSort.Turning)
            {
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0, 0.01f, 0), transform.forward / 2.5f - transform.right * Input.GetAxis("Horizontal"));
            }
            else if (sort == CheckingSort.Faling)
            {
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0, 0.01f, 0), -transform.forward / 2.5f);
            }

            targetPoint = ray.point;
            targetNormal = ray.normal;

            if (ray.canGoToPoint)
            {
                if(currentSort != Climbingsort.Climbing && currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    playerMove.enabled = false;
                    rigid.isKinematic = true;
                    //i might not have to do this one cuz mine checks distance but we will see
                    playerMove.isGrounded = false;
                }
                currentSort = Climbingsort.ClimbingTowardsPoint;
                beginDistance = Vector3.Distance(transform.position, (targetPoint - transform.rotation * handTrans.localPosition));
            }
        }
    }

    public RayInfo GetClosestPoint(Transform trans, Vector3 pos, Vector3 dir)
    {
        RayInfo curRay = new RayInfo();

        RaycastHit hit2;

        int oldLayer = trans.gameObject.layer;

        //change this
        trans.gameObject.layer = 14;

        if(Physics.Raycast(pos - dir, dir, out hit2,dir.magnitude * 2, currentSpotLayer))
        {
            curRay.point = hit2.point;
            curRay.normal = hit2.normal;

            //checks if you can go to point
            if (!Physics.Linecast(handTrans.position + transform.rotation * new Vector3(0,0.05f,-0.05f), curRay.point + new Vector3(0,0.05f,0),out hit2,checkLayersReachable))
            {//                                                                             the number below HERE needs to be half the with of the character
                if(!Physics.Linecast(curRay.point -  Quaternion.Euler(new Vector3(0,90,0)) * curRay.normal * characterWith + 0.1f * curRay.normal, curRay.point + Quaternion.Euler(new Vector3(0, 90, 0)) * curRay.normal * characterWith + 0.1f * curRay.normal, out hit2, checkLayersForObstacle))
                {
                    if (!Physics.Linecast(curRay.point + Quaternion.Euler(new Vector3(0, 90, 0)) * curRay.normal * characterWith + 0.1f * curRay.normal, curRay.point - Quaternion.Euler(new Vector3(0, 90, 0)) * curRay.normal * characterWith + 0.1f * curRay.normal, out hit2, checkLayersForObstacle))
                    {
                        curRay.canGoToPoint = true;
                    }
                    else
                    {
                        curRay.canGoToPoint = false;
                    }
                }
                else
                {
                    curRay.canGoToPoint = false;
                }
            }
            else
            {
                curRay.canGoToPoint = false;
            }
            trans.gameObject.layer = oldLayer;
            return curRay;
        }
        else
        {//messed up cant go to point
            
            trans.gameObject.layer = oldLayer;
            return curRay;
        }
    }

    public void MoveTowardsPoint()
    {

        transform.position = Vector3.Lerp(transform.position, (targetPoint - transform.rotation * handTrans.localPosition), Time.deltaTime * climbForce);

        Quaternion lookrotation = Quaternion.LookRotation(-targetNormal);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * climbForce);

        animator.SetBool("OnGround", false);

        float distance = Vector3.Distance(transform.position, (targetPoint - transform.rotation * handTrans.localPosition));
        float percent = -9 * (beginDistance - distance) / beginDistance;

        animator.SetFloat("Jump", percent);

        if(distance <= 0.01f && currentSort == Climbingsort.ClimbingTowardsPoint)
        {
            transform.position = targetPoint - transform.rotation * handTrans.localPosition;
            transform.rotation = lookrotation;

            lastTime = Time.time;
            currentSort = Climbingsort.Climbing;
        }

        if (distance <= 0.25f && currentSort == Climbingsort.ClimbingTowardsPlateau)
        {
            transform.position = targetPoint - transform.rotation * handTrans.localPosition;
            transform.rotation = lookrotation;

            lastTime = Time.time;
            currentSort = Climbingsort.Walking;

            rigid.isKinematic = false;
            tpuc.enabled = true;
        }
    }

    public void CheckForClimbStart()
    {
        RaycastHit hit2;

        Vector3 dir = transform.forward - transform.up / 0.8f;

        if(Physics.Raycast(transform.position + transform.rotation * raycastPos, dir , 1.6f) && !Input.GetButton("Jump"))
        {
            currentSort = Climbingsort.CheckingForClimbStart;
            if(Physics.Raycast(transform.position + new Vector3(0,1.1f,0) , -transform.up, out hit2, 1.6f, spotLayer))
            {
                FindSpot(hit2, CheckingSort.Faling);
            }
        }
    }

    public void CheckForPlateau()
    {
        RaycastHit hit2;
        Vector3 dir = transform.up + transform.forward / 2;

        if (!Physics.Raycast(handTrans.position + transform.rotation * verticalHandOffset, dir,out hit2,1.5f ,spotLayer))
        {
            currentSort = Climbingsort.ClimbingTowardsPlateau;

            if(Physics.Raycast(handTrans.position + dir * 1.5f, -Vector3.up, out hit2 , 1.7f, spotLayer))
            {
                targetPoint = handTrans.position + dir * 1.5f;
            }
            else
            {
                targetPoint = handTrans.position + dir * 1.5f - transform.rotation * new Vector3(0, -0.2f, 0.25f);
            }

            targetNormal = -transform.forward;

            animator.SetBool("Crouch", true);
            animator.SetBool("Onground", true);
        }
    }
}

[System.Serializable]
public enum Climbingsort
{
    Walking,
    Jumping,
    Falling,
    Climbing,
    ClimbingTowardsPoint,
    ClimbingTowardsPlateau,
    CheckingForClimbStart
}

[System.Serializable]
public class RayInfo
{
    public Vector3 point;
    public Vector3 normal;
    public bool canGoToPoint;

}

[System.Serializable]
public enum CheckingSort
{
    Normal,
    Turning,
    Faling
}