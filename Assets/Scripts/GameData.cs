using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    int deathCount;

    public int DeathCount { get => deathCount; set => deathCount = value; }

    // the values defined in this constructor will be the default values
    //the game starts with when there's no data to load
    public GameData()
    {
        deathCount = 0;
    }

    //public void ReceiveInventory(Inventory inv)
    //{

    //}

    public void ReceiveDeathCount(DeathCountScript death)
    {
        deathCount = death.deathCount;
    }
}
