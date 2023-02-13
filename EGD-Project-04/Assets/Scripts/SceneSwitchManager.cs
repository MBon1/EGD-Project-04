using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartScreen()
    {
        SceneManager.LoadScene("Start");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("RootTester");
    }
    public void CreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }
}
