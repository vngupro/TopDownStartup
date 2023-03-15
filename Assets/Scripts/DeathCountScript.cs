using UnityEngine;
using TMPro;
public class DeathCountScript : MonoBehaviour, IDataPersistence
{
    private int deathCount = 0;
    private TMP_Text deathCountText;
    public GameEventsManager gameEvents;
    private void Awake()
    {
        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCount.ToString();
        gameEvents.OnPlayerDeath += OnPlayerDeath;    
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = deathCount;
    }

    public void LoadData(GameData data)
    {
        deathCount = data.deathCount;
    }

    private void OnPlayerDeath()
    {
        deathCount++;
    }
}
