using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    private static bool created = false;

    public bool pcMode;
    public int collectiblesFound;
    public int alertedEnemies;
    public int deaths;

    public string endlvl;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void EndDemo(int collectibleAmount)
    {
        collectiblesFound = collectibleAmount;
        SceneManager.LoadScene(endlvl);
        Debug.Log(collectiblesFound);
    }

    public void Reset()
    {
        Destroy(gameObject);
    }
}
