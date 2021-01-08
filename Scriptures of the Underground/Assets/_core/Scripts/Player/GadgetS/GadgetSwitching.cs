using UnityEngine;

public class GadgetSwitching : MonoBehaviour
{

    public int selectedGadget = 0;
    public bool maskUnlocked;
    int previousSelectedGadget = 0;
    public bool playerObject;
    public FmodPlayerSounds sfx;

    // Start is called before the first frame update
    void Start()
    {
        SelectGadget();
    }

    // Update is called once per frame
    void Update()
    {
        previousSelectedGadget = selectedGadget;

        if(Input.GetButtonUp("RightBumper") && maskUnlocked)
        {
            if(selectedGadget >= transform.childCount - 1)
            {
                selectedGadget = 0;
            }
            else
            {
                selectedGadget++;
            } 
        }
        else if(Input.GetButtonUp("RightBumper") && !maskUnlocked)
        {
            if (selectedGadget >= transform.childCount - 2)
            {
                selectedGadget = 0;
            }
            else
            {
                selectedGadget++;
            }
        }

        if (Input.GetButtonUp("LeftBumper") && maskUnlocked)
        {
            if (selectedGadget <= 0 )
            {
                selectedGadget = transform.childCount - 1;
            }
            else
            {
                selectedGadget--;
            }
        }
        else if (Input.GetButtonUp("LeftBumper") && !maskUnlocked)
        {
            if (selectedGadget <= 0)
            {
                selectedGadget = transform.childCount - 2;
            }
            else
            {
                selectedGadget--;
            }
        }

        if(playerObject && Input.GetButtonUp("LeftBumper")|| playerObject && Input.GetButtonUp("RightBumper"))
        {
            sfx.CallGadgetSwitch();
        }

        if (previousSelectedGadget != selectedGadget)
        {
            SelectGadget();
        }
    }

    public void SelectGadget()
    {
        Debug.Log("TESTTTTTTTTT");
        int i = 0;
        foreach(Transform gadget in transform)
        {
            if(i== selectedGadget)
            {
                gadget.gameObject.SetActive(true);
            }
            else
            {
                gadget.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
