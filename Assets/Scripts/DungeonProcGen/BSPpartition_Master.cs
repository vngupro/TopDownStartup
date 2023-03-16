using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

//using Random = System.Random;


public class BSPpartition_Master : MonoBehaviour
{
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private int _maxDivision;
    [SerializeField] [Range(0, 100)] private int _splitStop;
    [SerializeField] private Vector2Int _minRoomSize;

    private Room_Master _roomMaster;

    public int SplitStop
    {
        get => _splitStop;
    }

    public int MaxDivision
    {
        get => _maxDivision;
    }

    public static BSPpartition_Master instance;

    private List<BSPpartition> _unitList = new List<BSPpartition>();

    private BSPpartition _root;

    void Awake()
    {
        instance = this;

        float w = _floorSize.x;
        float h = _floorSize.y;
        float x = 0;
        float y = 0;

        Camera.main.transform.position = new Vector3(x, y, -10f);

        Camera.main.orthographicSize = ((w > h * Camera.main.aspect) ? (float)w / (float)Camera.main.pixelWidth * Camera.main.pixelHeight : h) / 2;
    }

    void Start()
    {
        _roomMaster = GetComponent<Room_Master>();
        Generate();
    }

    void Update() //TODO
    {
        foreach (var unit in _unitList)
        {
            unit.DebugDrawUnit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    public void AddUnit(BSPpartition unit)
    {
        _unitList.Add(unit);
    }

    void Generate()
    {
        _root = new BSPpartition(Vector2.zero, _floorSize, null, 0, _roomMaster);
        _unitList.Add(_root);

        Connect();
    }

    void Restart()
    {
        Erase(_root);
        _unitList.Clear();
        Generate();
    }


    void Connect()
    {
        List<BSPpartition> connectList = new List<BSPpartition>();

        for (int i = 0; i < _unitList.Count; i++)
        {
            if (_unitList[i].Childs[0] != null && _unitList[i].Childs[0].Childs[0] == null) connectList.Add(_unitList[i]);
        }
    }


    void Erase(BSPpartition p)
    {
        if (p.Childs[0] != null)
        {
            Erase(p.Childs[0]);
            Erase(p.Childs[1]);
            Erase(p.Childs[2]);
            Erase(p.Childs[3]);
        }
        else
        {
            Destroy(p.Room.Render);
        }
    }
    /*
    BSPpartition GetLeafParent(BSPpartition p)
    {
        if (p.Childs[0] != null)
        {

        }
        else if (p.Childs[1] != null)
        {

        }
        else if (p.Childs[2] != null)
        {

        }
        else if (p.Childs[3] != null)
        {

        }
        return GetLeafParent(p.Childs[0]);
        else return p.Childs[0].Parent;
    }
    */
}

public class BSPpartition
{

    public BSPpartition(Vector2 center, Vector2 size, BSPpartition parent, int depth, Room_Master roomMaster)
    {
        _center = center;
        _extents = new Vector2(size.x / 2, size.y / 2);
        _depth = depth;
        _isRoot = parent == null;
        _roomMaster = roomMaster;

        Vector2Int sizeRound = new Vector2Int(Mathf.RoundToInt(size.x / 2), Mathf.RoundToInt(size.y / 2));

        if ((_depth >= BSPpartition_Master.instance.MaxDivision) ||
            (Random.Range(0, 101) < BSPpartition_Master.instance.SplitStop && !_isRoot))
        {
            Vector2 startVec = new Vector2(center.x - (_extents.x / 2), center.y + (_extents.y / 2));
            Vector2 genVec = new Vector2(center.x - _extents.x, center.y + _extents.y);

            _room = _roomMaster.Generate(sizeRound, genVec, startVec);

            return;
        }

        Split();
    }

    private BSPpartition _parent;
    private Vector2 _center;
    private Vector2 _extents;
    private int _depth = 0;
    private BSPpartition[] _childs = new BSPpartition[4];

    private bool _isRoot = false;

    private Room _room;
    private Room_Master _roomMaster;

    public Room Room
    {
        get => _room;
    }

    public BSPpartition[] Childs
    {
        get => _childs;
    }

    public BSPpartition Parent
    {
        get => _parent;
    }


    public void DebugDrawUnit()
    {
        Vector2 topLeft = new Vector2(_center.x - _extents.x, _center.y + _extents.y);
        Vector2 topRight = new Vector2(_center.x + _extents.x, _center.y + _extents.y);
        Vector2 bottomLeft = new Vector2(_center.x - _extents.x, _center.y - _extents.y);
        Vector2 bottomRight = new Vector2(_center.x + _extents.x, _center.y - _extents.y);



        Debug.DrawLine(topLeft, topRight);
        Debug.DrawLine(topLeft, bottomLeft);
        Debug.DrawLine(bottomLeft, bottomRight);
        Debug.DrawLine(topRight, bottomRight);
    }


    void Split()
    { 
        int depth = _depth + 1;
        _childs[0] = new BSPpartition(new Vector2(_center.x - (_extents.x / 2), _center.y + (_extents.y / 2)), _extents, this, depth, _roomMaster);
        _childs[1] = new BSPpartition(new Vector2(_center.x + (_extents.x / 2), _center.y + (_extents.y / 2)), _extents, this, depth, _roomMaster);
        _childs[2] = new BSPpartition(new Vector2(_center.x - (_extents.x / 2), _center.y - (_extents.y / 2)), _extents, this, depth, _roomMaster);
        _childs[3] = new BSPpartition(new Vector2(_center.x + (_extents.x / 2), _center.y - (_extents.y / 2)), _extents, this, depth, _roomMaster);
        BSPpartition_Master.instance.AddUnit(_childs[0]);
        BSPpartition_Master.instance.AddUnit(_childs[1]);
        BSPpartition_Master.instance.AddUnit(_childs[2]);
        BSPpartition_Master.instance.AddUnit(_childs[3]);
    }
}