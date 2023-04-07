using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;

[RequireComponent(typeof(Room_Master))]
public class BSP_Master : MonoBehaviour
{
    [SerializeField]                private Vector2Int _floorSize;
    [SerializeField]                private int _maxDivision;
    [SerializeField][Range(0, 100)] private int _splitStop;
    [SerializeField]                private int _minRoomSize;
    [SerializeField]                private int _maxRoomSize;



    private Room_Master _roomMaster;
    
    private Node _root;

    private bool _hasStairs = false;

    private Vector2 _spawnPoint;

    public Vector2 SpawnPoint
    {
        get => _spawnPoint;
        set => _spawnPoint = value;
    }

    public bool HasStairs
    {
        get => _hasStairs;
        set => _hasStairs = value;
    }

    public bool HasSpawnPoint { get; set; }

    public int MaxDivision => _maxDivision;

    public int MinRoomSize => _minRoomSize;

    public int SplitStop => _splitStop;

    public Room_Master RoomMaster => _roomMaster;

    void Start()
    {
        Init();
    }

    void Update()
    {
        _root.DebugDraw(true, Color.white);

        if (Input.GetKeyDown(KeyCode.Space)) ResetBSP();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D col = Physics2D.OverlapBox(mousePosition, new Vector2(0.25f, 0.25f), 0);
        Debug.Log(col?.name);
    }

    void Init()
    {
#if UNITY_EDITOR
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
#endif

        _roomMaster = GetComponent<Room_Master>();
        _root = new Node(null, new Rect(0, 0, _floorSize.x, _floorSize.y), 0, this);
        GenerateBSPNode(_root);
        _root.CreateRoom();
        GenerateCorridors(_root);
        _root.GenerateStairs();
        _root.GenerateSpawn();

        float w = _root.Rect.width;
        float h = _root.Rect.width;
        float x = _root.Rect.center.x;
        float y = _root.Rect.center.y;

        Camera.main.transform.position = new Vector3(x, y, -10f);

        Camera.main.orthographicSize = ((w > h * Camera.main.aspect) ? (float)w / (float)Camera.main.pixelWidth * Camera.main.pixelHeight : h) / 2;
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
                    obj.GetComponent<SpriteRenderer>().sprite = _roomMaster.CorridorTilesSpr[Random.Range(0, _roomMaster.CorridorTilesSpr.Count)];
                    obj.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    obj.transform.parent = transform;
                }
            }
        }
    }

    public void ResetBSP()
    {
        EraseNode(_root);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _hasStairs = false;

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
    }

    void OnDrawGizmos()
    {
        //if (_root != null) DebugDrawNodeGizmo(_root, Color.white);
    }

    void DebugDrawNodeGizmo(Node node, Color col)
    {
        Gizmos.color = col;
        Gizmos.DrawCube(node.Rect.center, (Vector3)node.Rect.size);
        if (node.LeftChild != null) DebugDrawNodeGizmo(node.LeftChild, Color.red);
        if (node.RightChild != null) DebugDrawNodeGizmo(node.RightChild, Color.blue);
    }
}

