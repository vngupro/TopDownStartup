using UnityEngine;
using TMPro;
public class DeathCountScript : MonoBehaviour, IDataPersistence
{
    public HealthModule playerHealthModule;


    [SaveField] private int deathCount = 0;
    private TMP_Text deathCountText;

    private void Awake()
    {
        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCount.ToString();
        DataPersistenceManager.Instance.Subscribe(this);
        
    }
    private void OnEnable()
    {
        playerHealthModule.Died += UpdateDeathCount;
        DataPersistenceManager.Instance.OnNewGame += ResetDeathCount;
    }

    private void OnDisable()
    {
        playerHealthModule.Died -= UpdateDeathCount;
        DataPersistenceManager.Instance.OnNewGame -= ResetDeathCount;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = deathCount;
    }

    public void LoadData(GameData data)
    {
        deathCount = data.deathCount;
        deathCountText.text = deathCount.ToString();
    }

    private void UpdateDeathCount()
    {
        deathCount++;
        deathCountText.text = deathCount.ToString();
    }

    private void ResetDeathCount()
    {
        deathCount = 0;
        deathCountText.text = deathCount.ToString();
    }
}
