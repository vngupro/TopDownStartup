using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System;
using System.Reflection;
using UnityEngine.InputSystem;

public class DataPersistenceManager : MonoBehaviour
{
    public event Action OnNewGame;

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "save.sav";

    public static DataPersistenceManager Instance { get; private set; }
    public List<IDataPersistence> dataPersistences = new List<IDataPersistence>();
    public Dictionary<IDataPersistence, object> keyValuePairs = new Dictionary<IDataPersistence, object>();
    private GameData _gameData;
    private FileDataHandler _dataHandler;
    private void Awake()
    {
        if(Instance != null)
        {
            UnityEngine.Debug.LogError("Found more than one Data Persistence Manager in the scene");
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
        BindingFlags flags =
             BindingFlags.Public |
             BindingFlags.Instance |
             BindingFlags.NonPublic;
        
        foreach (var data in dataPersistences
            .Select( i => (data: i, fields: i.GetType()
                .GetFields(flags)
                    .Where(j => j.GetCustomAttribute<SaveAttribute>() != null))))
        {
            foreach(var field in data.fields)
            {
                if (field.FieldType == typeof(bool))
                {
                    UnityEngine.Debug.Log(field.Name + " is bool");
                }
                if (field.FieldType == typeof(int))
                {
                    UnityEngine.Debug.Log(field.Name + " is int");
                    
                    SaveData saveData = new SaveData();
                    saveData.name = field.Name;
                    object obj = null;
                    keyValuePairs.TryGetValue(dataPersistences[0], out obj);
                    saveData.value = field.GetValue(obj);
                    UnityEngine.Debug.Log(saveData.value + " is value");

                    
                    if (_gameData.dico.ContainsKey(field.Name))
                    {
                        _gameData.dico[field.Name] = field.GetValue(obj);
                    }
                    else
                    {
                        _gameData.dico.Add(field.Name, field.GetValue(obj));
                    }
                }
                else if(field.FieldType == typeof(float))
                {
                    UnityEngine.Debug.Log(field.Name + " is float");
                }
                else if (field.FieldType == typeof(string))
                {
                    UnityEngine.Debug.Log(field.Name + " is string");
                }
                else if(field.FieldType.IsEnum)
                {
                    UnityEngine.Debug.Log(field.Name + " is enum");
                }
            }
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.SaveData(ref _gameData);
        }

        //// save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();
        // if no data cna be loaded, initialize to a new game
        if (_gameData == null)
        {
            UnityEngine.Debug.Log("No data was found. Initializing data to defaults.");
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

    public void Subscribe(IDataPersistence dataPersistence, object obj)
    {
        dataPersistences.Add(dataPersistence);
        //dataPersistence as object
        keyValuePairs.Add(dataPersistence, obj);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }

}
