using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private static ISaveLoadService _saveLoadService;
    private static IGameStateService _gameStateService;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;


    private void Awake()
    {
        _saveLoadService ??= Services.Resolve<ISaveLoadService>();
        _gameStateService ??= Services.Resolve<IGameStateService>();

        _gameStateService.RegisterPauseMenu(pausePanel);

        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
    }
    public void ResumeGame() => _gameStateService.PauseGame(false);

    private void Start()
    {
        saveButton.onClick.AddListener(OnSaveClicked);    
        loadButton.onClick.AddListener(OnLoadClicked);    
        quitButton.onClick.AddListener(OnQuitClicked);    
        restartButton.onClick.AddListener(OnRestartClicked);
        optionButton.onClick.AddListener(OnOptionClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnDestroy()
    {
        optionButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }

    public void OnBackClicked()
    {
        optionPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OnOptionClicked()
    {
        optionPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void OnRestartClicked()
    {
        _saveLoadService.NewGame();
    }

    public void OnSaveClicked()
    {
        _saveLoadService.Save();
    }

    public void OnLoadClicked()
    {
        _saveLoadService.Load();
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
