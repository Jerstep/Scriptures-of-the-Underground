using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControllsUIChanger : MonoBehaviour
{
    //values for the controllers
    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    //images to change and to be changed to
    public Image[] imageToChange;
    public Image[] xboxImage, pcImage;

    // Start is called before the first frame update
    void Start()
    {
        //find controller name and add the number to it
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

        //changes sprites based on the number 
        if (Xbox_One_Controller == 1)
        {
            
            for (int i = 0; i < imageToChange.Length; i++)
            {
                imageToChange[i].sprite = xboxImage[i].sprite;
            }
        }
        else if (PS4_Controller == 1)
        {
            //do something
        }
        else
        {
            for (int i = 0; i < imageToChange.Length; i++)
            {
                imageToChange[i].sprite = pcImage[i].sprite;
            }

        }
    }
}
