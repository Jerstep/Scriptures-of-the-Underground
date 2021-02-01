using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace SA
{
    public class CutsceneTrigger : MonoBehaviour
    {

        public PlayerStats player;
        public CinemachineVirtualCamera cutscenecam;
        public GameObject focusTarget, campos;
        public Animator anim;
        public ThirdPersonController tpc;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                cutscenecam.LookAt = focusTarget.transform;
                //cutscenecam.Follow = campos.transform;
                cutscenecam.ForceCameraPosition(campos.transform.position, cutscenecam.LookAt.rotation);
                player.CutsceneCamTurnOn();
                tpc.MoveToggle();
                StartCoroutine(StartScene());
                GetComponent<BoxCollider>().enabled = false;

            }
        }

        IEnumerator StartScene()
        {
            yield return new WaitForSeconds(1);
            anim.SetBool("Active", true);
        }


    }

}
