using System.Collections;
using UnityEngine;

public class Room
{
    private Rect       _rect;
    private GameObject _render;

    public GameObject Render => _render;
    public Rect Rect => _rect;

    public Room(Rect rect, GameObject render)
    {
        _rect = rect;
        _render = render;
    }
}