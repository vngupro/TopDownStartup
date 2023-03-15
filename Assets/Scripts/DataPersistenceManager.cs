using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    public List<IDataPersistence> dataPersistences = new List<IDataPersistence>();

    private GameData gameData;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void Start()
    {
        dataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistence in dataPersistences) 
        {
            dataPersistence.SaveData(ref gameData);
        }
        // TODO - pass the data to other scripts so they can update it

        // TODO - save that data to a file using the data handler
        Debug.Log("Saved death count = " + gameData.deathCount);
        
    }

    public void LoadGame()
    {
        // TO DO - Load any saved data form a file using the data handlser
        // if no data cna be loaded, initialize to a new game

        if (gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach(IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.LoadData(gameData);
        }

        Debug.Log("Loaded death count = " + gameData.deathCount);
        // TO DO - Push the loaded data to all other scrips that need it
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }
}
