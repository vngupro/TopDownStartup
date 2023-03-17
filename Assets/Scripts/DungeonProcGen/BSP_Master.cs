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
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private int _maxDivision;
    [SerializeField][Range(0, 100)] private int _splitStop;
    [SerializeField] private int _minRoomSize;
    [SerializeField] private int _maxRoomSize;

    public GameObject test;

    private Room_Master _roomMaster;
    
    private Node _root;


    public int MaxDivision
    {
        get => _maxDivision;
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
        Init();
    }

    void Update()
    {
        _root.DebugDraw(true, Color.white);

        if (Input.GetKeyDown(KeyCode.Space)) ResetBSP();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        test.transform.position = mousePosition;

        Collider2D col = Physics2D.OverlapBox(test.transform.position, new Vector2(0.25f, 0.25f), 0);
        Debug.Log(col?.name);
    }

    void Init()
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
#endif

        _roomMaster = GetComponent<Room_Master>();
        _root = new Node(null, new Rect(0, 0, _floorSize.x, _floorSize.y), 0, this);
        GenerateBSPNode(_root);
        _root.CreateRoom();
        GenerateCorridors(_root);

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


    void CreateCorridors(Node node)
    {
        if(_root.LeftChild == null) return;

        if (!node.IsLeaf)
        {
            if (node.LeftChild.IsLeaf && node.RightChild.IsLeaf) node.CreateCorridors(node.LeftChild, node.RightChild);
            else
            {
                CreateCorridors(node.LeftChild);
                CreateCorridors(node.RightChild);
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

    void ResetBSP()
    {
        EraseNode(_root);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Room_Master.HasStairs = false;

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
    #region Variables

    private Node _parent;
    public Node Parent
    {
        get => _parent;
    }

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

    private bool _isLeaf;
    public bool IsLeaf
    {
        get => _isLeaf;
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

    private List<Rect> _corridors = new List<Rect>();
    public List<Rect> Corridors
    {
        get => _corridors;
    }


    private enum SplitOrientation
    {
        HORIZONTAL,
        VERTICAL
    }

    private SplitOrientation _nodeSplit;

    #endregion



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
        }
        else
        {

            int splitDrift = Random.Range(_master.MinRoomSize, (int)(_rect.height - _master.MinRoomSize));
            
            _leftChild = new Node(this, new Rect(_rect.x, _rect.y, splitDrift, _rect.height), newDepth, _master);
            _rightChild = new Node(this, new Rect(_rect.x + splitDrift, _rect.y, _rect.width - splitDrift, _rect.height), newDepth, _master);
        }
        return true;
    }

    public void CreateRoom()
    {
        if (_leftChild != null) _leftChild.CreateRoom();

        if (_rightChild != null) _rightChild.CreateRoom();

        if (_leftChild != null && _rightChild != null) CreateCorridors(_leftChild, _rightChild);

        if (_leftChild == null && _rightChild == null)
        {
            int roomWidth = (int)Random.Range(_rect.width / 2, _rect.width - 2);
            int roomHeight = (int)Random.Range(_rect.height / 2, _rect.height - 2);
            int roomPosX = (int)Random.Range(1, _rect.width - roomWidth - 1);
            int roomPosY = (int)Random.Range(1, _rect.height - roomHeight - 1);

            Rect rect = new Rect(_rect.x + roomPosX, _rect.y + roomPosY, roomWidth, roomHeight);
            _room = _master.RoomMaster.Generate(rect);
            _isLeaf = true;
        }     
    }

    public Rect GetRoom()
    {
        if (_isLeaf)
        {
            return _room.Rect;
        }
        if (_leftChild != null)
        {
            Rect leftRoom = _leftChild.GetRoom();
            if (leftRoom.x != -1)
            {
                return leftRoom;
            }
        }
        if (_rightChild != null)
        {
            Rect rightRoom = _rightChild.GetRoom();
            if (rightRoom.x != -1)
            {
                return rightRoom;
            }
        }

        // workaround non nullable structs
        return new Rect(-1, -1, 0, 0);
    }

    public void CreateCorridors(Node left, Node right)
    {
        
        Rect lroom = left.GetRoom();
        Rect rroom = right.GetRoom();


        // attach the corridor to a random point in each room
        Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
        Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

        // always be sure that left point is on the left to simplify the code
        if (lpoint.x > rpoint.x)
        {
            Vector2 temp = lpoint;
            lpoint = rpoint;
            rpoint = temp;
        }

        int w = (int)(lpoint.x - rpoint.x);
        int h = (int)(lpoint.y - rpoint.y);


        // if the points are not aligned horizontally
        if (w != 0)
        {
            // choose at random to go horizontal then vertical or the opposite
            if (Random.Range(0, 1) > 2)
            {
                // add a corridor to the right
                _corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                // if left point is below right point go up
                // otherwise go down
                if (h < 0)
                {
                    _corridors.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    _corridors.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));
                }
            }
            else
            {
                // go up or down
                if (h < 0)
                {
                    _corridors.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    _corridors.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                }

                // then go right
                _corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1));
            }
        }
        else
        {
            // if the points are aligned horizontally
            // go up or down depending on the positions
            if (h < 0)
            {
                _corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));
            }
            else
            {
                _corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));
            }
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
