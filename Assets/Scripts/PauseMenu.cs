using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    private static ISaveLoadService _saveLoadService;

    [SerializeField] private Button muteButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;

    private void Awake() => _saveLoadService ??= Services.Resolve<ISaveLoadService>();

    private void Start()
    {
        saveButton.onClick.AddListener(OnSaveClicked);    
        loadButton.onClick.AddListener(OnLoadClicked);    
        quitButton.onClick.AddListener(OnQuitClicked);    
        restartButton.onClick.AddListener(OnRestartClicked);    
        muteButton.onClick.AddListener(OnMuteClicked);    
    }

    private void OnDestroy()
    {
        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        muteButton.onClick.RemoveAllListeners();
    }

    public void OnMuteClicked()
    {

    }

    public void OnRestartClicked()
    {

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
