using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerButtonController : MonoBehaviour
{
    public bool isSecret;
    public GameObject[] Door;

    public GameObject[] movePosition;
    public bool isTriggered;
    public GameplayUI UIScript;

    public GameObject fmodobject;

    // Start is called before the first frame update
    void Start()
    {
        UIScript = GameObject.Find("GameplayUi").GetComponent<GameplayUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            for(int i = 0; i < Door.Length; i++)
            {
                Door[i].transform.position = Vector3.MoveTowards(Door[i].transform.position, movePosition[i].transform.position, 3 * Time.deltaTime);
            }
            
        }
    }

    public void OpenDoor()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            fmodobject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isTriggered)
        {
            if (UIScript.interactUI != UIScript.interactUI.activeInHierarchy)
            {
                UIScript.ChangeUIText(false);
                UIScript.interactUI.SetActive(true);
            }

            if (Input.GetButtonDown("Interaction"))
            {
                OpenDoor();
            }
        }
        else if(other.tag == "Player" && isTriggered)
        {
            UIScript.interactUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (UIScript.interactUI == UIScript.interactUI.activeInHierarchy)
            {
                UIScript.interactUI.SetActive(false);
            }
        }
    }

}
