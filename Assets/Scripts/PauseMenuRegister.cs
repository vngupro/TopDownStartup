using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuRegister : MonoBehaviour
{
    private static IGameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService ??= Services.Resolve<IGameStateService>();
        _gameStateService.RegisterPauseMenu(gameObject);
        gameObject.SetActive(false);
    }

    public void ResumeGame() => _gameStateService.PauseGame(false);
}
