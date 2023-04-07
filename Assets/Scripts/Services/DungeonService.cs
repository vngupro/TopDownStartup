using System;
using UnityEngine;

public class DungeonService : MonoBehaviour, IDungeonService
{ 
    BSP_Master _bsp;
    private void Start() => _bsp = Instantiate(Resources.Load<GameObject>("Dungeon/Dungeon")).GetComponent<BSP_Master>();

    public void SpawnPlayers()
    {
        _bsp.OnDungeonFinishGenerate?.Invoke();
    }

    public void CreateDungeon()
    {
        _bsp.ResetBSP();
    }
}
