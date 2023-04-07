using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface ISaveLoadService
{
    void NewGame();
    void Save();
    void Load();
    void RegisterDTO(ref SaveLoadDTO dto);
}
