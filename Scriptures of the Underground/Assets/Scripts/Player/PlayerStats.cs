using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerStunItem;
    public int keys;

    Animator camAnim;

    public GameplayUI UI;
    bool overhead;

    public bool masked;

    // Start is called before the first frame update
    void Start()
    {
        camAnim = GameObject.Find("CamAnimator").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "hallwayTrigger")
        {
            CamToggle();
        }
    }

    public void StunItemUp()
    {
        playerStunItem++;
        UI.UpdateUIStun(playerStunItem);

    }

    public void CamToggle()
    {
        overhead = !overhead;
        camAnim.SetBool("Overhead", overhead);
    }

    public void MaskedFunction(bool _masked)
    {
        masked = _masked;
        Debug.Log("masked =" + masked);
    }
}
