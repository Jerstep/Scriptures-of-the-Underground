using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Text stunText;
    public string stunString;
    public PlayerStats player;

    public GameObject interactUI;
    public Image interactImage;

    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;
    public Image xboxImage, pcImage;

    // Start is called before the first frame update
    void Start()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }

        if (Xbox_One_Controller == 1)
        {
            //do something
            interactImage.sprite = xboxImage.sprite;

        }
        else if (PS4_Controller == 1)
        {
            //do something
        }
        else
        {
            // there is no controllers
            interactImage.sprite = pcImage.sprite;
        }
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
