using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : MonoBehaviour
{
    public float climbForce;
    public Climbingsort currentSort;
    public Transform handTrans;
    public Animator animator;
    public Rigidbody rigid;
    public CharacterController tpuc;

    private Vector3 targetPoint;
    private Vector3 targetNormal;

    private float lastTime;
    private float beginDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
