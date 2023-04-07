using UnityEngine;

internal interface IGameStateService
{
    void PauseGame(bool status);
    void RegisterPauseMenu(GameObject pauseMenu);
}
