using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
    }
    public void startGame()
    {
        SceneManager.LoadScene("testmap");
    }
}
