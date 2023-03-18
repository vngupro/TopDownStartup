using UnityEngine;
using TMPro;
public class DeathCountScript : MonoBehaviour, IDataPersistence
{
    public HealthModule playerHealthModule;
    public DataPersistenceManager dataPersistenceManager;

    private int deathCount = 0;
    private TMP_Text deathCountText;

    private void Awake()
    {
        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCount.ToString();
        playerHealthModule.OnDied += UpdateDeathCount;
        dataPersistenceManager.OnNewGame += ResetDeathCount;
        
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
