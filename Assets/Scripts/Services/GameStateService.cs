using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateService : MonoBehaviour, IGameStateService
{
    private GameObject _pauseMenu;
    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
            PauseGame(true);
        else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
            PauseGame(false);
    }

    public void PauseGame(bool status)
    {
        if (status)
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(status);
            _isPaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            _pauseMenu.SetActive(status);
            _isPaused= false;
        }
    }

    public void RegisterPauseMenu(GameObject pauseMenu) => _pauseMenu = pauseMenu;
}
