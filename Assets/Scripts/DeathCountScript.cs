using UnityEngine;
using TMPro;

public class DeathCountScript : MonoBehaviour
{
    public HealthModule playerHealthModule;

    private TMP_Text deathCountText;

    [System.Serializable]
    public class DeathCountDTO : SaveLoadDTO
    {
        //public int DeathCount = 0;
        //[field:SerializeField] public int DeathCount { get; set; }
    };

    [SerializeField] private SaveLoadDTO deathCountDTO = new();

    private void Awake()
    {
        Services.Resolve<ISaveLoadService>().RegisterDTO(ref deathCountDTO);

        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCountDTO.DeathCount.ToString();
    }

    private void Start()
    {
        playerHealthModule.Died += UpdateDeathCount;
    }

    private void Update()
    {
        Debug.Log("DeathCount Update = " + deathCountDTO.DeathCount);
        deathCountText.text = deathCountDTO.DeathCount.ToString();
    }
    private void OnDestroy()
    {
        playerHealthModule.Died -= UpdateDeathCount;
    }

    private void UpdateDeathCount()
    {
        deathCountDTO.DeathCount++;
        deathCountText.text = deathCountDTO.DeathCount.ToString();
    }

    private void ResetDeathCount()
    {
        deathCountDTO.DeathCount = 0;
        deathCountText.text = deathCountDTO.DeathCount.ToString();
    }
}