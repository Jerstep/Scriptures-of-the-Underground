using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Panda;

public class Enemy : MonoBehaviour
{

    public NavMeshAgent agent;

    public float rotationSpeed = 90;
    public float speed = 5;
    public float waitTime = 0.3f;
    //[Task]
    public bool foundplayer;

    [Range(0,100)]
    public float detectionMeter;
    public Image detectImage;

    public Transform pathHolder;
   // [Task]
    bool playerTargeted;

    Vector3[] waypoints;

    public Vector3 playerPos;
    public GameObject player;

    bool patrolling;

    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {

        // transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (foundplayer && detectionMeter < 100)
        {
            detectionMeter++;
        }
        else if (detectionMeter > 0)
        {
            detectionMeter--;
        }

        if (detectionMeter <= 0)
        {
            foundplayer = false;
            playerTargeted = false;
            Patrol();
        }

        if(detectionMeter >= 100)
        {
            playerTargeted = true;
            TargetPlayer();
        }

        detectImage.fillAmount = detectionMeter / 100;
    }

    //call when patrolling
    //[Task]
    public void Patrol()
    {
        if (!patrolling)
        {
            Debug.Log("we patrolling yo");
            StopCoroutine("FollowPath");
            waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
            }
            StartCoroutine(FollowPath(waypoints));
            patrolling = true;
            //Task.current.Succeed();
        }
    }

    //call when targeting player
    //[Task]
    public void TargetPlayer()
    {
        patrolling = false;
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        StartCoroutine(FollowPlayer(playerPos));
        //Task.current.Succeed();
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        //transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (!playerTargeted)
        {
            agent.SetDestination(targetWaypoint);
            Vector3 distanceToWalkPoint = transform.position - targetWaypoint;
            
            if (distanceToWalkPoint.magnitude < 1f)//transform.position == targetWaypoint)
            {
                Debug.Log("arrived");
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
        if (playerTargeted)
        {
            Debug.Log("player is targeted");
            StopCoroutine(FollowPath(waypoints));
        }
    }

    IEnumerator FollowPlayer(Vector3 waypoints)
    {
        //transform.position = waypoints;
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints;
        transform.LookAt(targetWaypoint);

        while (playerTargeted)
        {
            targetWaypoint = player.transform.position;
            agent.SetDestination(targetWaypoint);
            Vector3 distanceToWalkPoint = transform.position - targetWaypoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                
                targetWaypointIndex = (targetWaypointIndex + 1);
                targetWaypoint = waypoints;
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }

        if (!playerTargeted)
        {
            Patrol();
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {//trigonometry booooiiiiis
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x)* Mathf.Rad2Deg;
        //deltaangles calculates distance between angles
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y,targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

    
    
}
