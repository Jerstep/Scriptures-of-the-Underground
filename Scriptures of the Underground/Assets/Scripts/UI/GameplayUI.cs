using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Text stunText;
    public string stunString;
    public PlayerStats player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUIStun(int stun)
    {
        stunText.text = stun.ToString();
    }
}
