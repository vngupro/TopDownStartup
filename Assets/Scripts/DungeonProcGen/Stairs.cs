using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private BSP_Master _bsp;

    public BSP_Master BSP
    {
        get => _bsp;
        set => _bsp = value;
    }

    public void ClimbStairs()
    {
        _bsp.ResetBSP();
    }
}
