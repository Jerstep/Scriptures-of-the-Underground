using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Panda;
using SA;

public class Enemy : MonoBehaviour
{
    public States enemyStates = States.Patrol;

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
    public bool playerTargeted;

    Vector3[] waypoints;

    public Vector3 playerPos;
    public GameObject player;

    bool patrolling;

    bool stunned;
    float stunTime = 3;

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

        switch (enemyStates)
        {
            case States.Patrol:
                Patrol();
                if (foundplayer && detectionMeter < 100)
                    enemyStates = States.PlayerTarget;
                if (stunned)
                {
                    enemyStates = States.Stunned;
                }
                break;
            case States.PlayerTarget:
                if (detectionMeter <= 0)
                {
                    foundplayer = false;
                    playerTargeted = false;
                    enemyStates = States.Patrol;
                }
                else if (detectionMeter >= 100)
                {
                    playerTargeted = true;
                    enemyStates = States.PlayerAttack;
                }
                if (stunned)
                {
                    enemyStates = States.Stunned;
                }
                break;
            case States.PlayerAttack:
                TargetPlayer();
                if (detectionMeter <= 0)
                {
                    foundplayer = false;
                    playerTargeted = false;
                    enemyStates = States.Patrol;
                }
                if (stunned)
                {
                    enemyStates = States.Stunned;
                }
                break;
            case States.Distracted:
                patrolling = false;
                if (stunned)
                {
                    enemyStates = States.Stunned;
                }
                break;
            case States.Stunned:
                StartCoroutine(Stunned());
                break;
            case States.CapturedPlayer:
                Debug.Log("playercaptured");
                break;
        }

        detectImage.fillAmount = detectionMeter / 100;
    }

    //call when patrolling
    //[Task]
    public void Patrol()
    {
        if (!foundplayer && detectionMeter > 0)
        {
            detectionMeter--;
        }

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

    public void Distracted(Vector3 _bulletPos)
    {
        enemyStates = States.Distracted;
        Debug.Log("enemy script distraction");
        Vector3 bulletPos = new Vector3(_bulletPos.x, transform.position.y, _bulletPos.z);
        StartCoroutine(FollowDistracion(bulletPos));
        //StopAllCoroutines();
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

    IEnumerator FollowDistracion(Vector3 waypoints)
    {
        //transform.position = waypoints;
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints;
        transform.LookAt(targetWaypoint);

        while (enemyStates == States.Distracted)
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
                //doesnt swtich yet
                yield return new WaitForSeconds(10);
                enemyStates = States.Patrol;

            }
            
            yield return null;
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


    IEnumerator Stunned()
    {
        Debug.Log("Stunned");
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        enemyStates = States.Patrol;
    }

    public void CapturedPlayer(ThirdPersonController player)
    {
        foundplayer = false;
        playerTargeted = false;
        patrolling = false;
        StopAllCoroutines();
        agent.velocity = Vector3.zero;
        player.enabled = false;
        enemyStates = States.CapturedPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //CapturedPlayer(other.GetComponent<ThirdPersonController>());
            other.GetComponent<ThirdPersonController>().Respawn();
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

    public enum States
    {
        Patrol,
        PlayerTarget,
        PlayerAttack,
        LostPlayer,
        Distracted,
        Stunned,
        CapturedPlayer
    }
    
}
