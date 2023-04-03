using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
 
public class GridManager : MonoBehaviour
{
    // If you have more layers to handle then do a List<Tilemap>
    public Tilemap tilemap;
 
    // If you have a lot of tiles, think about some list, dictionary or structure
    public TileBase tileLand;
    public TileBase tileWater;

    // Sample terrain to be generated
    List<List<int>> gameWorld = new List<List<int>>
    {
        new List<int> { 0, 0, 0, 0, 0},
        new List<int> { 0, 1, 1, 1, 0},
        new List<int> { 0, 1, 1, 1, 0},
        new List<int> { 0, 1, 1, 1, 0},
        new List<int> { 0, 0, 0, 0, 0},
    };
 
    void Start()
    {
        for(int x = 0; x < gameWorld.Count; x++)
        {
            for(int y = 0; y < gameWorld[x].Count; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), (gameWorld[x][y] == 0 ? tileWater : tileLand));
            }
        }
    }
}