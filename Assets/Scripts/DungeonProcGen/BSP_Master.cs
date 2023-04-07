using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Room_Master))]
public class BSP_Master : MonoBehaviour
{
    [SerializeField]                private Vector2Int _floorSize;
    [SerializeField]                private int        _maxDivision;
    [SerializeField][Range(0, 100)] private int        _splitStop;
    [SerializeField]                private int        _minRoomSize;
    [SerializeField]                private int        _maxRoomSize;

    private static Vector2 _dLeft  = new Vector2(-0.5f, -0.5f);
    private static Vector2 _dRight = new Vector2(0.5f, -0.5f);
    private static Vector2 _uLeft  = new Vector2(-0.5f, 0.5f);
    private static Vector2 _uRight = new Vector2(0.5f, 0.5f);

    private static IDungeonService _dungeonService;

    private Room_Master _roomMaster;
    
    private Node _root;

    private bool _hasStairs = false;

    private GameObject _collisionsParent;

    private Camera _cam;
    public Action OnDungeonFinishGenerate { get; set; } = () => { };
    public bool HasStairs
    {
        get => _hasStairs;
        set => _hasStairs = value;
    }
    
    public bool HasSpawnPoint { get; set; }
    public Vector2 SpawnPoint { get; set; }

    public int MaxDivision => _maxDivision;

    public int MinRoomSize => _minRoomSize;

    public int SplitStop => _splitStop;

    public Room_Master RoomMaster => _roomMaster;

    public List<bool> TilesArray { get; private set; }

    public bool GetTile(int x, int y) => TilesArray[_floorSize.x * x + y];
    public void SetTile(int x, int y) => TilesArray[_floorSize.x * x + y] = true;

    private void Awake()
    {
        _collisionsParent = new GameObject("Collisions");
        _dungeonService ??= Services.Resolve<IDungeonService>();
    }

    void Start()
    {
        _cam = Camera.main;
        Init();
    }

    void Update()
    {

#if UNITY_EDITOR
        _root.DebugDraw(true, Color.white);
        if (Input.GetKeyDown(KeyCode.Space)) ResetBSP();
#endif
    }

    void Init()
    {
#if UNITY_EDITOR
        // Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        // System.Type type = assembly.GetType("UnityEditor.LogEntries");
        // MethodInfo method = type.GetMethod("Clear");
        // method.Invoke(new object(), null);
#endif
        TilesArray = Enumerable.Repeat(false, _floorSize.x * _floorSize.y).ToList();

        _roomMaster = GetComponent<Room_Master>();
        _root = new Node(null, new Rect(0, 0, _floorSize.x, _floorSize.y), 0, this);
        GenerateBSPNode(_root);
        _root.CreateRoom();
        GenerateCorridors(_root);
        _root.GenerateStairs();
        GenerateCollisions();
        _root.GenerateSpawn();
        
        _dungeonService.SpawnPlayers();
        
#if UNITY_EDITOR
        //DebugPrintRoom();
#endif
        
        //This code serve as debug, to fit the whole dungeon into the cam render 
        // float w = _root.Rect.width;
        // float h = _root.Rect.width;
        // float x = _root.Rect.center.x;
        // float y = _root.Rect.center.y;
        //
        // _cam.transform.position = new Vector3(x, y, -10f);
        //
        // _cam.orthographicSize = ((w > h * _cam.aspect) ? (float)w / (float)_cam.pixelWidth * _cam.pixelHeight : h) / 2;
    }

    void GenerateBSPNode(Node node)
    {
        if (node.LeftChild == null && node.RightChild == null)
        {
            if (node.Rect.width > _maxRoomSize || node.Rect.height > _maxRoomSize)
            {
                if (node.Split())
                {
                    GenerateBSPNode(node.LeftChild);
                    GenerateBSPNode(node.RightChild);
                }
            }
        }
        
    }


    void GenerateCorridors(Node node)
    {
        if (node == null) return;

        GenerateCorridors(node.LeftChild);
        GenerateCorridors(node.RightChild);

        foreach (Rect corr in node.Corridors)
        {
            for (int i = (int)corr.x; i < corr.xMax; ++i)
            {
                for (int j = (int)corr.y; j < corr.yMax; ++j)
                {
                    GameObject obj = Instantiate(_roomMaster.CorridorSprite, new Vector3(i, j, 0f), Quaternion.identity);
                    SetTile(i, j);
                    obj.GetComponent<SpriteRenderer>().sprite = _roomMaster.CorridorTilesSpr[Random.Range(0, _roomMaster.CorridorTilesSpr.Count)];
                    obj.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    obj.transform.parent = transform;
                }
            }
        }
    }

    void GenerateCollisions()
    {
        for (int i = _floorSize.y - 1; i >= 0; --i)
        {
            for (int j = _floorSize.x - 1; j >= 0; --j)
            {

                if (!GetTile(j, i))
                {
                    var arr = new(Vector2 pointOne, Vector2 pointTwo) [4];
                    if (GetTile(j, Math.Max(i - 1, 0)))                arr[0] = (_dLeft, _dRight);
                    if (GetTile(j, Math.Min(i + 1, _floorSize.y - 1))) arr[1] = (_uLeft, _uRight);
                    if (GetTile(Math.Min(j + 1, _floorSize.x - 1), i)) arr[2] = (_uRight, _dRight);
                    if (GetTile(Math.Max(j - 1, 0), i))                arr[3] = (_uLeft, _dLeft);
                        
                    
                    if ((arr[0] != (Vector2.zero, Vector2.zero)) || 
                        (arr[1] != (Vector2.zero, Vector2.zero)) || 
                        (arr[2] != (Vector2.zero, Vector2.zero)) || 
                        (arr[3] != (Vector2.zero, Vector2.zero)))
                        CreateCollisions(arr, j, i);
                }
            }
        }
    }

    void CreateCollisions((Vector2 pointOne, Vector2 pointTwo)[] arr, int X, int Y)
    {
        GameObject colGo = new GameObject("Col: (" + X + ", " + Y + ")");
        colGo.transform.parent = _collisionsParent.transform;
        colGo.transform.position = new Vector2(X, Y);
        foreach (var vecTup in arr)
        {
            if (!(vecTup.pointOne == Vector2.zero && vecTup.pointTwo == Vector2.zero))
            {
                EdgeCollider2D col = colGo.AddComponent<EdgeCollider2D>();
                col.points = new Vector2[] { vecTup.pointOne, vecTup.pointTwo };
            }
        }

        SpriteRenderer sr = colGo.AddComponent<SpriteRenderer>();
        sr.sprite = _roomMaster.WallTilesSpr[Random.Range(0, _roomMaster.WallTilesSpr.Count())];
    }

    public void ResetBSP()
    {
        EraseNode(_root);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform col in _collisionsParent.transform)
        {
            Destroy(col.gameObject);
        }

        TilesArray.Clear();
        
        _hasStairs = false;
        HasSpawnPoint = false;

        Init();
    }

    void EraseNode(Node node)
    {
        //GC will pick up any lost Node
        if (node.LeftChild != null)
        {
            EraseNode(node.LeftChild);
            EraseNode(node.RightChild);
        }
        else if (node != null)
        {
            Destroy(node.Room.Render);    
        }
    }

    public void CreateStairs(Vector2 vec, Room parent)
    {
        GameObject stairsGo = Instantiate(_roomMaster.Stairs, new Vector3(vec.x, vec.y, 0), Quaternion.identity);
        stairsGo.transform.parent = parent.Render.transform;
        stairsGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        stairsGo.GetComponent<Stairs>().BSP = this;
    }

    // void OnDrawGizmos()
    // {
    //     if (_root != null) DebugDrawNodeGizmo(_root, Color.white);
    // }
    //
    // void DebugDrawNodeGizmo(Node node, Color col)
    // {
    //     Gizmos.color = col;
    //     Gizmos.DrawCube(node.Rect.center, (Vector3)node.Rect.size);
    //     if (node.LeftChild != null) DebugDrawNodeGizmo(node.LeftChild, Color.red);
    //     if (node.RightChild != null) DebugDrawNodeGizmo(node.RightChild, Color.blue);
    // }
    
    // void DebugPrintRoom()
    // {
    //     string str = "";
    //     for (int i = _floorSize.y - 1; i >= 0; --i)
    //     {
    //         for (int j = _floorSize.x - 1; j >= 0; --j)
    //         {
    //             bool tile = GetTile(j, i);
    //             str += tile ? "<color=#ff0000ff>X</color>" : "0";
    //         }
    //
    //         str += "\n";
    //     }
    //     Debug.Log(str);
    // }
}

