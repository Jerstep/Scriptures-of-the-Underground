using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerPressurePlateContoller : MonoBehaviour
{
    public bool isSecret;
    public GameObject Door;

    public GameObject startPos, movePosition;
    public bool isTriggered;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, movePosition.transform.position, 3 * Time.deltaTime);
        }
        else
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, startPos.transform.position, 3 * Time.deltaTime);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy" || other.tag == "Player")
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
        {
            isTriggered = false;
        }
    }

    public void openDoor()
    {
        if (!isTriggered)
        {
            isTriggered = true;
        }
    }
}
