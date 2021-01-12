using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndSceneStatDisplay : MonoBehaviour
{
    public Gamemanager gman;

    public TMP_Text storiesValue, DeathsValue, DetectionValue;

    // Start is called before the first frame update
    void Start()
    {
        if(gman == null)
        {
            gman = GameObject.Find("GameManager").GetComponent<Gamemanager>();
            storiesValue.text = gman.collectiblesFound.ToString();
            DeathsValue.text = gman.deaths.ToString();
            DetectionValue.text = gman.alertedEnemies.ToString();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
