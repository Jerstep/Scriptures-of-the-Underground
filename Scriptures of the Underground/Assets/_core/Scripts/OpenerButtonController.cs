using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerButtonController : MonoBehaviour
{
    public bool isSecret;
    public GameObject Door;

    public GameObject movePosition;
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
    }

    public void openDoor()
    {
        if (!isTriggered)
        {
            isTriggered = true;
        }
    }

}
