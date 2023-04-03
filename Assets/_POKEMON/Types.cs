using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Types : MonoBehaviour
{
    public TypeSet[] types;
}

[Serializable]
public class TypeSet
{
    public string english;
    public string chinese;
    public string japanese;
}
