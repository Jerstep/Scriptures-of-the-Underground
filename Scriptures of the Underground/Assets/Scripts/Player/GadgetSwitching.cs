using UnityEngine;

public class GadgetSwitching : MonoBehaviour
{

    public int selectedGadget = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectGadget();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedGadget = selectedGadget;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
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

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
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

        if(previousSelectedGadget != selectedGadget)
        {
            SelectGadget();
        }
    }

    public void SelectGadget()
    {
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
