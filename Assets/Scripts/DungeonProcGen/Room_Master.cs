using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Room_Master : MonoBehaviour
{
    [SerializeField] private GameObject _tileSprite;


    public List<Room> _rooms = new List<Room>();

    public Room Generate(Vector2 size, Vector2 center, Vector2 start)
    {
        Debug.Log(size);
        GameObject roomGo = new GameObject(center.ToString());
        roomGo.transform.position = center;
        Vector2 startPos = Vector2.zero + new Vector2(Mathf.RoundToInt(start.x) - 0.5f, Mathf.RoundToInt(start.y) - 0.5f);
        Vector2 currPos = startPos;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject tile = Instantiate(_tileSprite);
                tile.transform.parent = roomGo.transform;
                tile.transform.position = currPos;
                currPos.y--;
            }

            currPos.y = startPos.y;
            currPos.x++;
        }

        Room room = new Room(size, center, roomGo);
        _rooms.Add(room);
        return room;
    }
}


public class Room
{
    private Vector2 _size;
    private Vector2 _center;
    private GameObject _render;

    public GameObject Render
    {
        get => _render;
    }

    public Room(Vector2 size, Vector2 center, GameObject render)
    {
        _size = size;
        _center = center;
        _render = render;
    }
}