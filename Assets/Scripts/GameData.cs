using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string name;
    public object value;
}

[System.Serializable]
public class GameData
{
    public Dictionary<string, object> dico = new Dictionary<string, object>();
    //public List<SaveData> datas = new List<SaveData>();
    //private int deathCount;

    //public int DeathCount { get => deathCount; set => deathCount = value; }

    // the values defined in this constructor will be the default values
    //the game starts with when there's no data to load
    public GameData()
    {
        //deathCount = 0;
    }

    //public void ReceiveInventory(Inventory inv)
    //{

    //}

    public void ReceiveDeathCount(DeathCountScript death)
    {
        //deathCount = death.deathCount;
    }
}
