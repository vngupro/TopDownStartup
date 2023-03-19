using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System;
using System.Reflection;

public class DataPersistenceManager : MonoBehaviour
{
    public event Action OnNewGame;

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "save.sav";

    public static DataPersistenceManager Instance { get; private set; }
    public List<IDataPersistence> dataPersistences = new List<IDataPersistence>();

    private GameData _gameData;
    private FileDataHandler _dataHandler;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Reset()
    {
        fileName = "save.sav";
    }
    public void Start()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        //dataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    [Button]
    public void NewGame()
    {
        _gameData = new GameData();
        OnNewGame?.Invoke();
    }

    public void SaveGame()
    {
        foreach(var el in dataPersistences.Select(i => (data:i, fields:i.GetType()
                .GetFields(System.Reflection.BindingFlags.Instance)
                .Where(j => j.CustomAttributes.FirstOrDefault(k => k is SaveAttribute) != null))))
        {

            foreach(FieldInfo f in el.fields)
            {
                //switch(f.FieldType.GetType())
                //{
                //    case typeof(int) intData:

                //        break;
                //}

                if(f.FieldType.GetType() == typeof(int))
                {

                }
                else if(f.FieldType.GetType() == typeof(bool))
                {

                }

            }
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistence in dataPersistences) 
        {
            dataPersistence.SaveData(ref _gameData);
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();
        // if no data cna be loaded, initialize to a new game
        if (_gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.LoadData(_gameData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void Subscribe(IDataPersistence dataPersistence)
    {
        dataPersistences.Add(dataPersistence);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }

}
