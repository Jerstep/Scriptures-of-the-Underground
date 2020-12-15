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
        FmodPlayerSounds playerSounds;

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
            if (maskCharge <= 0)
            {
                masked = false;

            }
            else if (masked)
            {
                maskCharge -= 5 * Time.deltaTime;
            }

            if (Input.GetButtonDown("Inventory"))
            {
                JournalCamToggle();
            }
        }

        IEnumerator RechargeMask()
        {
            yield return new WaitForSeconds(rechargeTimer);
            maskCharge = 100;
            if (maskObject.activeSelf)
            {
                masked = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PersefShrine")
            {
                ost.PersefTheme();
            }

            if (other.tag == "EnemyZone")
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

            if (other.tag == "EnemyZone")
            {
                ost.EnemyFar();
            }
        }

        public void BulletsItemUp()
        {
            bullets += 1;
            UI.UpdateUIBullet(bullets);

        }

        public void OverheadCamToggle()
        {
            overhead = !overhead;
            camAnim.SetBool("Aiming", false);
            camAnim.SetBool("Overhead", overhead);
        }

        public void CutsceneCamTurnOn()
        {
            camAnim.SetBool("Overhead", false);
            camAnim.SetBool("Cutscene", true);
        }

        public void CutsceneCamTurnOff()
        {
            camAnim.SetBool("Cutscene", false);
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
            if (maskCharge <= 0 && masked == false)
            {
                StartCoroutine(RechargeMask());
            }
            Debug.Log("masked =" + masked);
        }

        public void EndingGame()
        {
            gameman.EndDemo(notesTaken);
        }

       
    }