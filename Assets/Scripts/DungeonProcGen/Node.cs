using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

class Node
{
    #region Variables

    private Node _parent;
    private Node _leftChild;
    private Node _rightChild;
    private bool _isLeaf;
    private Rect _rect;
    private int  _depth;

    private BSP_Master _master;

    private Room _room;

    private List<Rect> _corridors = new List<Rect>();
    public Node Parent => _parent;

    public Node LeftChild => _leftChild;

    public Node RightChild => _rightChild;

    public bool IsLeaf => _isLeaf;

    public Rect Rect => _rect;

    public Room Room
    {
        get => _room;
        set => _room = value;
    }

    public List<Rect> Corridors => _corridors;


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
            for (int x = (int)rect.x; x < rect.xMax; ++x)
            {
                for (int y = (int)rect.y; y < rect.yMax; ++y)
                {
                    _master.SetTile(x, y);
                }
            }
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

    public void GenerateStairs()
    {
        if (_master.HasStairs) return;

        if (_isLeaf)
        {
            _master.HasStairs = true;
            int x = (int)Random.Range(_room.Rect.xMin, _room.Rect.xMax);
            int y = (int)Random.Range(_room.Rect.yMin, _room.Rect.yMax);
            _master.CreateStairs(new Vector2(x, y), _room);
        }

        if (_leftChild != null && _rightChild == null) _leftChild.GenerateStairs();
        else if (_leftChild == null && _rightChild != null) _rightChild.GenerateStairs();
        else if (_leftChild != null && _rightChild != null)
        {
            if (Random.Range(0f, 1f) < 0.5f) _leftChild.GenerateStairs();
            else _rightChild.GenerateStairs();
        }
    }
    
    
    public void GenerateSpawn()
    {
        if (_master.HasSpawnPoint) return;
        
        if (_isLeaf)
        {
            _master.HasSpawnPoint = true;
            int x = (int)Random.Range(_room.Rect.xMin, _room.Rect.xMax);
            int y = (int)Random.Range(_room.Rect.yMin, _room.Rect.yMax);
            _master.SpawnPoint = new Vector2(x, y);
            // GameObject go = new GameObject();
            // go.transform.position = new Vector2(x, y);
            // go.transform.localScale = new Vector3(5.5f, 5.5f, 1);
            // go.AddComponent<SpriteRenderer>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
            // go.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }

        if (_leftChild != null && _rightChild == null) _leftChild.GenerateSpawn();
        else if (_leftChild == null && _rightChild != null) _rightChild.GenerateSpawn();
        else if (_leftChild != null && _rightChild != null)
        {
            if (Random.Range(0f, 1f) < 0.5f) _leftChild.GenerateSpawn();
            else _rightChild.GenerateSpawn();
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

