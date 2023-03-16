using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

[RequireComponent(typeof(Room_Master))]
public class BSP_Master : MonoBehaviour
{
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private int _maxDivision;
    [SerializeField][Range(0, 100)] private int _splitStop;
    [SerializeField] private Vector2 _maxDrift;
    [SerializeField] private int _minRoomSize;
    [SerializeField] private int _maxRoomSize;



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

    public int MinRoomSize
    {
        get => _minRoomSize;
    }

    public int SplitStop
    {
        get => _splitStop;
    }

    public Room_Master RoomMaster
    {
        get => _roomMaster;
    }

    void Start()
    {
        _roomMaster = GetComponent<Room_Master>();
        _root = new Node(null, new Rect(0, 0, _floorSize.x, _floorSize.y), 0, this);
        GenerateBSPNode(_root);
        _root.CreateRoom();
    }

    void Update()
    {
        _root.DebugDraw(true, Color.white);

        if (Input.GetKeyDown(KeyCode.Space)) Reset();
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

    void Reset()
    {
        EraseNode(_root);
        _root = new Node(null, new Rect(0, 0, _floorSize.x, _floorSize.y), 0, this);
        GenerateBSPNode(_root);
        _root.CreateRoom();
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


class Node
{
    private Node _parent;

    private Node _leftChild;
    public Node LeftChild
    {
        get => _leftChild;
    }

    private Node _rightChild;
    public Node RightChild
    {
        get => _rightChild;

    }

    private Rect _rect;
    public Rect Rect
    {
        get => _rect;
    }


    private int _depth;

    private BSP_Master _master;

    private Room _room;
    public Room Room 
    {
        get => _room;
        set => _room = value;    
    }

    private enum SplitOrientation
    {
        HORIZONTAL,
        VERTICAL
    }

    private SplitOrientation _nodeSplit;

    public Node(Node parent, Rect rect, int depth, BSP_Master master)
    {
        _parent = parent;
        _rect = rect;
        _master = master;
        _depth = depth;
    }

    public bool Split()
    {
        //Test cases to determine if node will be a room or split
        //Test if maximum node depth is reached 
        if (_depth >= _master.MaxDivision)
            return false;
        //Test if node is already small
        if (Mathf.Min(_rect.width, _rect.height) / 2 < _master.MinRoomSize)
            return false;
        //Random stop
        if (Random.Range(0, 101) < _master.SplitStop)
            return false;
  

        //Determine split orientation by this node rect (ie: if too large, split vertically)
        if ((_rect.width / _rect.height) >= 1.25f) //Take ratio and test if it's superior to one and a quarter
            _nodeSplit = SplitOrientation.VERTICAL;
        else if ((_rect.height / _rect.width) >= 1.25f) //Same but inverse ratio
            _nodeSplit = SplitOrientation.HORIZONTAL;
        else//If node is quite squared, just randomize
        {
            if (Random.Range(0.0f, 1.0f) >= 0.5f) 
                _nodeSplit = SplitOrientation.HORIZONTAL;
            else 
                _nodeSplit = SplitOrientation.VERTICAL;

        } 
            

        int newDepth = _depth + 1;

        if (_nodeSplit == SplitOrientation.HORIZONTAL)
        {
            int splitDrift = Random.Range(_master.MinRoomSize, (int)(_rect.width - _master.MinRoomSize));
            
            _leftChild = new Node(this, new Rect(_rect.x, _rect.y, _rect.width, splitDrift), newDepth, _master);
            _rightChild = new Node(this, new Rect(_rect.x, _rect.y + splitDrift, _rect.width, _rect.height - splitDrift), newDepth, _master);
            
            
            /*
            float drift = Random.Range(-_master.MaxDrift.x, _master.MaxDrift.x);

            Vector2 leftCenter = new Vector2(_center.x - (_extents.x / 2) + drift, _center.y);
            Vector2 rightCenter = new Vector2(_center.x + (_extents.x / 2) + drift, _center.y);

            Vector2 leftSize = new Vector2(_extents.x + (drift * 2), _extents.y * 2);
            Vector2 rightSize = new Vector2(_extents.x - (drift * 2), _extents.y * 2);

            //Debug.Log("V Left size: " + leftSize);
            //Debug.Log("V Right size: " + rightSize);

            _leftChild = new Node(this, leftCenter, leftSize, newDepth, _master);
            _rightChild = new Node(this, rightCenter, rightSize, newDepth, _master);
            */
        }
        else
        {

            int splitDrift = Random.Range(_master.MinRoomSize, (int)(_rect.height - _master.MinRoomSize));
            
            _leftChild = new Node(this, new Rect(_rect.x, _rect.y, splitDrift, _rect.height), newDepth, _master);
            _rightChild = new Node(this, new Rect(_rect.x + splitDrift, _rect.y, _rect.width - splitDrift, _rect.height), newDepth, _master);

            /*
            float drift = Random.Range(-_master.MaxDrift.y, _master.MaxDrift.y);

            Vector2 leftCenter = new Vector2(_center.x, _center.y - (_extents.y / 2) + drift);
            Vector2 rightCenter = new Vector2(_center.x, _center.y + (_extents.y / 2) + drift);

            Vector2 leftSize = new Vector2(_extents.x * 2, _extents.y + (drift * 2));
            Vector2 rightSize = new Vector2(_extents.x * 2, _extents.y - (drift * 2));

            //Debug.Log("H Left size: " + leftSize);
            //Debug.Log("H Right size: " + rightSize);

            _leftChild = new Node(this, leftCenter, leftSize, newDepth, _master);
            _rightChild = new Node(this, rightCenter, rightSize, newDepth, _master);
            */
        }

        return true;
    }

    public void CreateRoom()
    {
        if (_leftChild != null) _leftChild.CreateRoom();

        if (_rightChild != null) _rightChild.CreateRoom();

        if (_leftChild == null && _rightChild == null)
        {
            int roomWidth = (int)Random.Range(_rect.width / 2, _rect.width - 2);
            int roomHeight = (int)Random.Range(_rect.width / 2, _rect.width - 2);
            int roomPosX = (int)Random.Range(1, _rect.width - roomWidth - 1);
            int roomPosY = (int)Random.Range(1, _rect.height - roomHeight - 1);

            Rect rect = new Rect(_rect.x + roomPosX, _rect.y + roomPosY, roomWidth, roomHeight);
            _room = _master.RoomMaster.Generate(rect);
        }     
    }

    public void DebugDraw(bool drawChilds, Color drawColor)
    {
        
        Vector2 topLeft = new Vector2(_rect.xMin, _rect.yMin);
        Vector2 topRight = new Vector2(_rect.xMax, _rect.yMin);
        Vector2 bottomLeft = new Vector2(_rect.xMin, _rect.yMax);
        Vector2 bottomRight = new Vector2(_rect.xMax, _rect.yMax);

        Debug.DrawLine(topLeft, topRight, drawColor);
        Debug.DrawLine(topLeft, bottomLeft, drawColor);
        Debug.DrawLine(bottomLeft, bottomRight, drawColor);
        Debug.DrawLine(topRight, bottomRight, drawColor);


        if (drawChilds)
        {
            _leftChild?.DebugDraw(true, Color.red);
            _rightChild?.DebugDraw(true, Color.blue);
        }
        
    }

    
}
