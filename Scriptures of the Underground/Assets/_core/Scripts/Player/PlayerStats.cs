using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerStunItem;
    public int keys;
    public int bullets;

    Animator camAnim;

    public GameplayUI UI;
    bool overhead;

    public bool masked;
    public float maskCharge = 100;
    public float rechargeTimer;
    public GameObject maskObject;
    public GameObject respawnLocation;

    public GameObject LvlOstHolder;
    FmodOst ost;

    public int notesTaken;

    Gamemanager gameman;

    bool journalActive;

    public InventoryUI journalUI;

    // Start is called before the first frame update
    void Start()
    {
        camAnim = GameObject.Find("CamAnimator").GetComponent<Animator>();
        ost = GameObject.Find("fmod-ost").GetComponent<FmodOst>();
        gameman = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        //journalUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(maskCharge <= 0 && masked == true)
        {
            StartCoroutine(RechargeMask());
        }

        if (Input.GetButtonDown("Inventory"))
        {
            JournalCamToggle();
        }
    }

    IEnumerator RechargeMask()
    {
        masked = false;
        yield return new WaitForSeconds(rechargeTimer);
        maskCharge = 100;
        if (maskObject.activeSelf)
        {
            masked = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "PersefShrine")
        {
            ost.PersefTheme();
        }

        if (other.tag == "Enemy")
        {
            ost.EnemyClose();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PersefShrine")
        {
            ost.LvlMainTheme();
        }

        if (other.tag == "Enemy")
        {
            ost.EnemyFar();
        }
    }

    public void StunItemUp()
    {
        playerStunItem++;
        UI.UpdateUIStun(playerStunItem);

    }

    public void OverheadCamToggle()
    {
        overhead = !overhead;
        camAnim.SetBool("Aiming", false);
        camAnim.SetBool("Overhead", overhead);
    }

    public void AimCamTurnOn()
    {
        camAnim.SetBool("Overhead", false);
        camAnim.SetBool("Aiming", true);
    }

    public void AimCamTurnOff()
    {
        camAnim.SetBool("Aiming", false);
    }

    public void JournalCamToggle()
    {
        journalActive = !journalActive;
        camAnim.SetBool("Journal", journalActive);
        journalUI.ToggleInventoryUi();
    }

    public void MaskedFunction(bool _masked)
    {
        masked = _masked;
        Debug.Log("masked =" + masked);
    }

    public void EndingGame()
    {
        gameman.EndDemo(notesTaken);
    }


}
