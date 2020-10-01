using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public float rotationSpeed;
    public bool foundplayer;

    [Range(0,100)]
    public float detectionMeter;
    public Image detectImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (foundplayer && detectionMeter < 100)
        {
            detectionMeter++;
        }
        else if(detectionMeter > 0)
        {
            detectionMeter--;
        }

        detectImage.fillAmount = detectionMeter / 100;
    }
}
