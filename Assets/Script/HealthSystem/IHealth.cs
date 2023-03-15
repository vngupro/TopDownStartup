using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    void Damage(int amount);
    void Regen(int amount);
    void Kill();

}
