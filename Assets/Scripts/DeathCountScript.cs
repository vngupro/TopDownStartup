using UnityEngine;
using TMPro;
using System;

/*
 * ISaveLoadService
 * pass un dto 
 * sache juste que tu dois save ca donc passe en ref
 * load c'est la meme chose que save à l'envers
 * 
 */
public class DeathCountScript : MonoBehaviour, IDataPersistence
{
    public HealthModule playerHealthModule;

    [Save] [SerializeField] private int deathCount = 0;

    private TMP_Text deathCountText;

    private void Awake()
    {
        deathCountText = GetComponentInChildren<TMP_Text>();
        deathCountText.text = deathCount.ToString();
        DataPersistenceManager.Instance.Subscribe(this);
        // try cast to object
    }

    private void Start()
    {

        playerHealthModule.Died += UpdateDeathCount;
        DataPersistenceManager.Instance.OnNewGame += ResetDeathCount;
    }

    private void OnDestroy()
    {
        playerHealthModule.Died -= UpdateDeathCount;
        DataPersistenceManager.Instance.OnNewGame -= ResetDeathCount;
    }

    public void SaveData(ref GameData data)
    {
        data.ReceiveDeathCount(this);
    }

    public void LoadData(GameData data)
    {
        deathCount = Convert.ToInt32(data.dico["deathCount"]);
        deathCountText.text = deathCount.ToString();
    }

    private void UpdateDeathCount()
    {
        deathCount++;
        deathCountText.text = deathCount.ToString();
        DataPersistenceManager.Instance.SaveGame();
    }

    private void ResetDeathCount()
    {
        deathCount = 0;
        deathCountText.text = deathCount.ToString();
    }
}