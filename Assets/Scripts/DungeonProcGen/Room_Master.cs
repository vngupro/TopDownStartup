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
    [SerializeField] private List<Sprite> _roomTilesSpr     = new List<Sprite>();
    [SerializeField] private List<Sprite> _corridorTilesSpr = new List<Sprite>();
    [SerializeField] private GameObject   _tileSprite;
    [SerializeField] private GameObject   _corridorSprite;
    [SerializeField] private GameObject   _stairs;

    public GameObject TileSprite => _tileSprite;

    public GameObject CorridorSprite => _corridorSprite;

    public List<Sprite> CorridorTilesSpr => _corridorTilesSpr;

    public GameObject Stairs => _stairs;

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
            }
        }

        Room room = new Room(rect, roomGo);
        return room;
    }

}

