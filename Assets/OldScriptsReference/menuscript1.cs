using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class menuscript1 : MonoBehaviour
{
    public MovementR move;
    public GameObject menu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeSelf)
            {
                move.movementEnabled = true;
                Time.timeScale = 1;
                menu.SetActive(false);
            }
            else
            {
                move.movementEnabled = false;
                Time.timeScale = 0;
                menu.SetActive(true);
            }
        }
    }
    public void Resume()
    {
        move.movementEnabled = true;
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    public void MainMenu()
    {
        move.movementEnabled = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("menu");
    }
}
