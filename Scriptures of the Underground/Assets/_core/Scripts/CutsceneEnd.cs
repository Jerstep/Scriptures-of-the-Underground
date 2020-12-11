using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CutsceneEnd : MonoBehaviour
    {
        public PlayerStats player;
        public ThirdPersonController tpc;

        public void EndScene()
        {
            player.CutsceneCamTurnOff();
            tpc.MoveToggle();
        }
    }
}

