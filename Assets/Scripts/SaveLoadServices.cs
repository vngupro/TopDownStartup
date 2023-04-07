using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

/*
 * WHY NOT REF  ?????
 * */
internal sealed class SaveLoadService : MonoBehaviour, ISaveLoadService
{

    public List<SaveLoadDTO> datas = new();
    
    private FileDataHandler _dataHandler; 
    private string fileName = "save.sav";
    private GameData _gameData;

    public void Start()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Load();
    }

    public void Save()
    {
        Debug.Log("Save");
        _gameData.datas = datas;
        _dataHandler.Save(_gameData);
    }

    public void Load()
    {
        Debug.Log("Load");

        if (_gameData == null)
        {
            UnityEngine.Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        else
        {
            _gameData = _dataHandler.Load();
            datas.ElementAt(0).DeathCount = _gameData.datas.ElementAt(0).DeathCount;
        }
    }

    public void NewGame() 
    {
        _gameData = new();
    }

    public void RegisterDTO(ref SaveLoadDTO dto)
    {
        Debug.Log("Register DTO");
        datas.Add(dto);
    }
}