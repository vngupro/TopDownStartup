using UnityEngine;
using TMPro;
public class DeathCountScript : MonoBehaviour, IDataPersistence
{
    public TestDeathCountInvoker OnPlayerDeathInvoker;

    private int deathCount = 0;
    private TMP_Text deathCountText;

    private void Awake()
    {
        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCount.ToString();
        OnPlayerDeathInvoker.OnPlayerDeath += OnPlayerDeath;
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

    private void OnPlayerDeath()
    {
        deathCount++;
        deathCountText.text = deathCount.ToString();
    }
}
