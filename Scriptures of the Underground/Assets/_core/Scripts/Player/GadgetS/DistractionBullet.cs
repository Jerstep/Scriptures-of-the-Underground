using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionBullet : MonoBehaviour
{
    public int collisionCount = 0;
    public int enemyColCount = 0;
    int pulseTime = 2;
    public float speed;
    public GameObject posMarker;
    public Enemy enemy;
    

    Rigidbody myRigid;

    public bool IsColliding()
    {
        if(collisionCount == 1)
        {
            return true;
        }
        return false;
    }

    public bool enemyDistracted()
    {
        if (enemyColCount == 1)
        {
            return true;
        }
        return false;
    }

    bool enemyReached;

    bool isDistracting;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        myRigid = GetComponent<Rigidbody>();
        StartCoroutine(CheckColllision());
        myRigid.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Enviorment")
        {
            Debug.Log("EVIORMENT");
            //if collides with anything do the distraction
            
            myRigid.velocity = Vector3.zero;

            
        }
        collisionCount = 1;
        if (!isDistracting)
            StartCoroutine(Distraction());


        if (other.tag == "Enemy")
        {
            Debug.Log("Distraction ");
            enemy = other.GetComponent<Enemy>();
            if (!enemy.foundplayer || !enemy.playerTargeted)
            {
                Debug.Log("Distraction ");
                enemyColCount = 1;
            }
        }

        
    }

    IEnumerator Distraction()
    {
        
        
        isDistracting = true;

        yield return new WaitForSeconds(pulseTime);
        //check if something is in the range
        GetComponent<CapsuleCollider>().enabled = true;
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            Debug.Log("Distraction fucntion");
            Vector3 bulletPos = new Vector3(posMarker.transform.position.x, posMarker.transform.position.y, posMarker.transform.position.z);
            enemy.Distracted(bulletPos);
        }
        yield return new WaitForSeconds(pulseTime);
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(pulseTime);
        GetComponent<CapsuleCollider>().enabled = true;
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            Vector3 bulletPos = new Vector3(posMarker.transform.position.x, posMarker.transform.position.y, posMarker.transform.position.z);
            enemy.Distracted(bulletPos);
        }
        yield return new WaitForSeconds(pulseTime);
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(pulseTime);
        GetComponent<CapsuleCollider>().enabled = true;
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            Vector3 bulletPos = new Vector3(posMarker.transform.position.x, posMarker.transform.position.y, posMarker.transform.position.z);
            enemy.Distracted(bulletPos);
        }
        else
        {
            StartCoroutine(Destroy());
        }
        yield return new WaitForSeconds(pulseTime);
        GetComponent<CapsuleCollider>().enabled = false;


    }

    IEnumerator CheckColllision()
    {
        //destroy gameobject
        yield return new WaitForSeconds(10);
        if (!IsColliding())
        {
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        //destroy gameobject
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
