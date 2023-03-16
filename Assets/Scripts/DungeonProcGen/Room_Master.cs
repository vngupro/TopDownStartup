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

    public GameObject TileSprite
    {
        get => _tileSprite;
    }

    public Room Generate(Rect rect)
    {
        GameObject roomGo = new GameObject(rect.position.ToString());
        roomGo.transform.position = rect.position;

        for (int x = (int)rect.x; x < rect.xMax; ++x)
        {
            for (int y = (int)rect.y; y < rect.yMax; ++y)
            {
                GameObject tile = Instantiate(_tileSprite, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.parent = roomGo.transform;
            }
        }

        Room room = new Room(rect, roomGo);
        return room;
        
        /*
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

        Room room = new Room(_rect, roomGo);
        _rooms.Add(room);
        return room;
        */
    }
}


public class Room
{
    private Rect _rect;
    private GameObject _render;

    public GameObject Render
    {
        get => _render;
    }

    public Rect Rect
    {
        get => _rect;
    }

    public Room(Rect rect, GameObject render)
    {
        _rect = rect;
        _render = render;
    }
}