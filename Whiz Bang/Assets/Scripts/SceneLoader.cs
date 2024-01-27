using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject gameUI, pauseUI;

    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameUI.activeInHierarchy)
                {
                    gameUI.SetActive(false);
                    pauseUI.SetActive(true);
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                { 
                    gameUI.SetActive(true);
                    pauseUI.SetActive(false);
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
    public void ButtonStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ButtonMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
}
