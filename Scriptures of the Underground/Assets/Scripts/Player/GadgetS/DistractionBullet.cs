using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionBullet : MonoBehaviour
{
    public int collisionCount = 0;
    public int enemyColCount = 0;
    public Enemy enemy;

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
        StartCoroutine(CheckColllision());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if collides with anything do the distraction
        collisionCount = 1;

        

        if(other.tag == "Enemy")
        {
            Debug.Log("Distraction ");
            enemy = other.GetComponent<Enemy>();
            if (!enemy.foundplayer || !enemy.playerTargeted)
            {
                Debug.Log("Distraction ");
                enemyColCount = 1;
            }
        }

        if (!isDistracting)
            StartCoroutine(Distraction());
    }

    IEnumerator Distraction()
    {
        //check if something is in the range
        GetComponent<CapsuleCollider>().enabled = true;
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isDistracting = true;

        yield return new WaitForSeconds(2);
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            Debug.Log("Distraction fucntion");
            enemy.Distracted(bulletPos);
        }
        yield return new WaitForSeconds(2);
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            enemy.Distracted(bulletPos);
        }
        yield return new WaitForSeconds(2);
        if (enemyDistracted() && !enemyReached)
        {
            enemyReached = true;
            // change its target to the bullet location
            enemy.Distracted(bulletPos);
        }
        else
        {
            StartCoroutine(Destroy());
        }
        
        
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
