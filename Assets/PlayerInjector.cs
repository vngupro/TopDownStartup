using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInjector : MonoBehaviour
{
    [SerializeField] Entity _e;
    [SerializeField] PlayerReference _ref;

    ISet<Entity> RealRef => _ref;

    public IReadOnlyList<int> T { get => t; }

    List<int> t;

    void Awake()
    {
        //_ref.Set(_e);
        RealRef.Set(_e);
    }

}
