using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject cursorMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SmoothingPanel.isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            cursorMenu.SetActive(false);
        }
        else if (SmoothingPanel.isActive) pauseMenu.SetActive(false);
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        cursorMenu.SetActive(true);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
