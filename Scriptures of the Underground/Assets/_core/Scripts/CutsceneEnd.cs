using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public PlayerStats player;

    public void EndScene()
    {
        player.CutsceneCamTurnOff();
    }
}
