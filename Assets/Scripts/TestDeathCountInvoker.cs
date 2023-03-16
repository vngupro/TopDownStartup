using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeathCountInvoker : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public bool CheatPlayerDead = false;

    private void Update()
    {
        if(CheatPlayerDead)
        {
            OnPlayerDeath?.Invoke();
            CheatPlayerDead= false;
        }
    }
}
