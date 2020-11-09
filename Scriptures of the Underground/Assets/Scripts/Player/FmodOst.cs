using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodOst : MonoBehaviour
{
    FMODUnity.StudioEventEmitter ost;

    // Start is called before the first frame update
    void Start()
    {
        ost = GetComponent<FMODUnity.StudioEventEmitter>();
        // ost.setParameterByName("Distance to Shrine", 1);
        
    }


    public void PersefTheme()
    {
        ost.SetParameter("Distance to Shrine", 0f);
    }

    public void LvlMainTheme()
    {
        ost.SetParameter("Distance to Shrine", 100);
    }

    
}
