using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Room_Master : MonoBehaviour
{
    [SerializeField] private List<Sprite> _roomTilesSpr = new List<Sprite>();
    [SerializeField] private List<Sprite> _corridorTilesSpr = new List<Sprite>();
    [SerializeField] private GameObject _tileSprite;
    [SerializeField] private GameObject _corridorSprite;
    [SerializeField] private GameObject _Stairs;

    private static bool _hasStairs = false;

    public static bool HasStairs
    {
        get => _hasStairs;
        set => _hasStairs = value;
    }

    public GameObject TileSprite
    {
        get => _tileSprite;
    }

    public GameObject CorridorSprite
    {
        get => _corridorSprite;
    }

    public List<Sprite> CorridorTilesSpr
    {
        get => _corridorTilesSpr;
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
                tile.GetComponent<SpriteRenderer>().sprite = _roomTilesSpr[Random.Range(0, _roomTilesSpr.Count)];
                tile.transform.parent = roomGo.transform;
                if (!_hasStairs && Random.Range(0f, 100f) < 20f)
                {
                    _hasStairs = true;
                    GameObject stairsGo = Instantiate(_Stairs, new Vector3(x, y, 0), Quaternion.identity);
                    stairsGo.transform.parent = roomGo.transform;
                    stairsGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }

            }
        }

        Room room = new Room(rect, roomGo);
        return room;
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