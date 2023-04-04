using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items
{
    public int id;
    public ItemEntry name;
}

[System.Serializable]
public class ItemEntry
{
    public string japanese;
    public string english;
    public string chinese;
}
