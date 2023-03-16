using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class BSP_Master : MonoBehaviour
{
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private int _maxDivision;
    [SerializeField][Range(0, 100)] private int _splitStop;
    [SerializeField] private Vector2Int _minRoomSize;
    [SerializeField] private Vector2 _maxDrift;

    private Room_Master _roomMaster;
    
    private Node _root;

    public int MaxDivision
    {
        get => _maxDivision;
    }

    public Vector2 MaxDrift
    {
        get => _maxDrift;
    }


    void Start()
    {
        _roomMaster = GetComponent<Room_Master>();
        GenerateBSP();
    }

    void Update()
    {
        _root.DebugDraw(true, UnityEngine.Color.white);

        if (Input.GetKeyDown(KeyCode.Space)) GenerateBSP();
    }

    void GenerateBSP()
    {
        _root = new Node(null, Vector2.zero, _floorSize, 0, this);
        CreateRooms(_root);
    }

    void CreateRooms(Node node)
    {

        if (node.LeftChild != null)
        {
            CreateRooms(node.LeftChild);
            CreateRooms(node.RightChild);
        }
        else
        {
        Debug.Log("Create Rooms");
            Vector2 startVec = new Vector2(node.Center.x - (node.Extents.x / 2), node.Center.y + (node.Extents.y / 2));
            Vector2Int sizeRound = new Vector2Int(Mathf.RoundToInt(node.Extents.x), Mathf.RoundToInt(node.Extents.y));
            Vector2 genVec = new Vector2(node.Center.x - node.Extents.x, node.Center.y + node.Extents.y);
            Debug.Log(sizeRound);
            _roomMaster.Generate(sizeRound, genVec, startVec);
        }
    }
}


class Node
{
    private Node _parent;
    private Node _leftChild;
    private Node _rightChild;

    public Node LeftChild
    {
        get => _leftChild;
    }
    public Node RightChild
    {
        get => _rightChild;

    }

    public Vector2 Center
    {
        get => _center;
    }

    public Vector2 Extents
    {
        get => _extents;
    }

    private Vector2 _center;
    private Vector2 _extents;

    private int _depth;

    private BSP_Master _master;

    private enum SplitOrientation
    {
        HORIZONTAL,
        VERTICAL
    }

    private SplitOrientation _nodeSplit;

    public Node(Node parent, Vector2 center, Vector2 size, int depth, BSP_Master master)
    {
        Debug.Log("Gen");
        _parent = parent;
        _center = center;
        _extents = size / 2;
        _master = master;
        _depth = depth;

        if (depth >= _master.MaxDivision) return;

        Split();
    }

    void Split()
    {
        _nodeSplit = (SplitOrientation)Random.Range(0, 2);

        int newDepth = _depth + 1;
        /*
        if (_depth % 2 == 0) orient = SplitOrientation.VERTICAL;
        else orient = SplitOrientation.HORIZONTAL;
        */
        if (_nodeSplit == SplitOrientation.VERTICAL)
        {
            float drift = Random.Range(-_master.MaxDrift.x, _master.MaxDrift.x);

            Vector2 leftCenter = new Vector2(_center.x - (_extents.x / 2) + drift, _center.y);
            Vector2 rightCenter = new Vector2(_center.x + (_extents.x / 2) + drift, _center.y);

            Vector2 leftSize = new Vector2(_extents.x + (drift * 2), /*_center.y + (_extents.y * 2)*/_extents.y * 2);
            Vector2 rightSize = new Vector2(_extents.x - (drift * 2), /*_center.y + (_extents.y * 2)*/_extents.y * 2);

            //Debug.Log("V Left size: " + leftSize);
            //Debug.Log("V Right size: " + rightSize);

            _leftChild = new Node(this, leftCenter, leftSize, newDepth, _master);
            _rightChild = new Node(this, rightCenter, rightSize, newDepth, _master);
        }
        else
        {
            float drift = Random.Range(-_master.MaxDrift.y, _master.MaxDrift.y);

            Vector2 leftCenter = new Vector2(_center.x, _center.y - (_extents.y / 2) + drift);
            Vector2 rightCenter = new Vector2(_center.x, _center.y + (_extents.y / 2) + drift);

            Vector2 leftSize = new Vector2(/*_center.x + (_extents.x * 2)*/_extents.x * 2, _extents.y + (drift * 2));
            Vector2 rightSize = new Vector2(/*_center.x + (_extents.x * 2)*/_extents.x * 2, _extents.y - (drift * 2));

            //Debug.Log("H Left size: " + leftSize);
            //Debug.Log("H Right size: " + rightSize);

            _leftChild = new Node(this, leftCenter, leftSize, newDepth, _master);
            _rightChild = new Node(this, rightCenter, rightSize, newDepth, _master);
        }
    }

    public void DebugDraw(bool drawChilds, UnityEngine.Color drawColor)
    {
        Vector2 topLeft = new Vector2(_center.x - _extents.x, _center.y + _extents.y);
        Vector2 topRight = new Vector2(_center.x + _extents.x, _center.y + _extents.y);
        Vector2 bottomLeft = new Vector2(_center.x - _extents.x, _center.y - _extents.y);
        Vector2 bottomRight = new Vector2(_center.x + _extents.x, _center.y - _extents.y);

        Debug.DrawLine(topLeft, topRight, drawColor);
        Debug.DrawLine(topLeft, bottomLeft, drawColor);
        Debug.DrawLine(bottomLeft, bottomRight, drawColor);
        Debug.DrawLine(topRight, bottomRight, drawColor);

        if (drawChilds)
        {
            _leftChild?.DebugDraw(true, UnityEngine.Color.red);
            _rightChild?.DebugDraw(true, UnityEngine.Color.blue);
        }
    }
}
