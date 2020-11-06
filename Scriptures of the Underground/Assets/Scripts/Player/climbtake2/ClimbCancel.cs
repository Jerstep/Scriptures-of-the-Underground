using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class ClimbCancel : MonoBehaviour
{
    Freeclimb climbScript;

    // Start is called before the first frame update
    void Start()
    {
        climbScript = GetComponentInParent<Freeclimb>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CancelClimb")
        {
            climbScript.CancelClimb();
        }
    }
}
