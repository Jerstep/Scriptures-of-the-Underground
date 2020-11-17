using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Text stunText;
    public string stunString;
    public PlayerStats player;

    Gamemanager gameMan;

    public GameObject interactUI;
    public GameObject interactImage;

    public Image pcIcon, conIcon;

    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        /*if (gameMan.pcMode)
        {
            interactUI.GetComponent<Image>().sprite = pcIcon.sprite;
        }
        else
        {
            interactUI.GetComponent<Image>().sprite = conIcon.sprite;
        }*/
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
