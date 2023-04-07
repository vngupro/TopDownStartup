using System.Transactions.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private static ISaveLoadService _saveLoadService;
    private static IGameStateService _gameStateService;
    private static IAudioService _audioService;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;


    private void Awake()
    {
        _saveLoadService ??= Services.Resolve<ISaveLoadService>();
        _gameStateService ??= Services.Resolve<IGameStateService>();
        _audioService ??= Services.Resolve<IAudioService>();

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
        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
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

    public void OnMasterChanged(float volume)
    {
        _audioService.SetMasterVolume(masterSlider);
    }
    
    public void OnBGMChanged(float volume)
    {
        _audioService.SetBGMVolume(bgmSlider);
    }

    public void OnSFXChanged(float volume)
    {
        _audioService.SetSFXVolume(sfxSlider);
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
