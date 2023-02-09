using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] Tilemap rootTileMap;
    [SerializeField] Vector3Int originRootPosition = new Vector3Int(0, 4, 0);

    [Header("Roots")]
    Dictionary<Tile, Root> rootsByTile = new Dictionary<Tile, Root>();

    [SerializeField] Root originRoot;
    [SerializeField] Root verticalRoot;
    [SerializeField] Root horizontalRoot;
    [Space(10)]
    [SerializeField] Root crossRoot;
    [Space(10)]
    [SerializeField] Root leftTRoot;
    [SerializeField] Root rightTRoot;
    [SerializeField] Root upTRoot;
    [SerializeField] Root downTRoot;
    [Space(10)]
    [SerializeField] Root leftUpLRoot;
    [SerializeField] Root rightUpLRoot;
    [SerializeField] Root leftDownLRoot;
    [SerializeField] Root rightDownLRoot;
    [Space(20)]

    [SerializeField] List<Vector3Int> growthPoints = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> bendPoints = new List<Vector3Int>();


    // Start is called before the first frame update
    void Start()
    {
        rootsByTile.Add(originRoot.tile, originRoot);
        rootsByTile.Add(verticalRoot.tile, verticalRoot);
        rootsByTile.Add(horizontalRoot.tile, horizontalRoot);

        rootsByTile.Add(crossRoot.tile, crossRoot);

        rootsByTile.Add(leftTRoot.tile, leftTRoot);
        rootsByTile.Add(rightTRoot.tile, rightTRoot);
        rootsByTile.Add(upTRoot.tile, upTRoot);
        rootsByTile.Add(downTRoot.tile, downTRoot);

        rootsByTile.Add(leftUpLRoot.tile, leftUpLRoot);
        rootsByTile.Add(rightUpLRoot.tile, rightUpLRoot);
        rootsByTile.Add(leftDownLRoot.tile, leftDownLRoot);
        rootsByTile.Add(rightDownLRoot.tile, rightDownLRoot);

        // Set initial root position
        AddBaseRoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3Int ContactPointToPosition(Vector3Int position, Direction contactPoint)
    {
        if (contactPoint == Direction.Up)
        {
            return position + new Vector3Int(0, 1, 0);
        }
        else if (contactPoint == Direction.Down)
        {
            return position - new Vector3Int(0, 1, 0);
        }
        else if (contactPoint == Direction.Left)
        {
            return position - new Vector3Int(1, 0, 0);
        }
        else
        {
            return position + new Vector3Int(1, 0, 0);
        } 
    }

    bool HasContactPoint(Direction[] directions, Direction direction)
    {
        foreach(Direction d in directions)
        {
            if (d == direction)
            {
                return true;
            }
        }
        return false;
    }

    void AddRoot(Root root, Vector3Int position, bool addToGrowthPoints = true)
    {
        rootTileMap.SetTile(position, root.tile);

        // Check if root has a down contact point (downward growth)
        if (addToGrowthPoints && HasContactPoint(root.contactPoints, Direction.Down))
        {
            growthPoints.Add(position);
        }
    }

    void RemoveRoot(Vector3Int position)
    {
        Tile root = rootTileMap.GetTile<Tile>(position);

        if (root == null)
        {
            return;
        }

        rootTileMap.SetTile(position, null);

        if (growthPoints.Contains(position))
        {
            growthPoints.Remove(position);
        }
    }

    [ContextMenu("Add Base Root")]
    void AddBaseRoot()
    {
        AddRoot(originRoot, originRootPosition);
    }

    
    [ContextMenu("Grow")]
    void Grow()
    {
        List<Vector3Int> newGrowthPoints = new List<Vector3Int>();

        foreach (Vector3Int point in growthPoints)
        {
            Vector3Int targetPoint = ContactPointToPosition(point, Direction.Down);
            if (rootTileMap.GetTile(targetPoint) == null)
            {
                // Populate
                newGrowthPoints.Add(targetPoint);
                AddRoot(verticalRoot, targetPoint, false);
            }
            else
            {
                // Root is blocked. Check again later
                newGrowthPoints.Add(point);
            }
        }

        growthPoints = newGrowthPoints;
    }


    public bool CheckIfTRootPossible(Vector3Int position, Direction direction)
    {
        Tile tile = rootTileMap.GetTile<Tile>(position);

        if (tile == null)
        {
            return false;
        }

        if (direction == Direction.Left || direction == Direction.Right)
        {
            if (tile == verticalRoot.tile)
            {
                return true;
            }
        }
        else
        {
            if (tile == horizontalRoot.tile)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeToTRoot(Vector3Int position, Direction direction)
    {
        if (CheckIfTRootPossible(position, direction))
        {
            RemoveRoot(position);
            if (direction == Direction.Left)
            {
                AddRoot(leftTRoot, position);
            }
            else if (direction == Direction.Right)
            {
                AddRoot(rightTRoot, position);
            }
            else if (direction == Direction.Up)
            {
                AddRoot(upTRoot, position);
            }
            else
            {
                AddRoot(downTRoot, position);
            }
        }
    }

    public void ChangeToLRoot()
    {

    }

    public void ChangeToCrossRoot()
    {

    }

    /*
    void Bend(Vector3Int position, Direction direction)
    {
        TileBase tileBase = rootTileMap.GetTile(position);

        Vector3Int endL = position + new Vector3Int(1, 0, 0);
        Vector3Int endR = position - new Vector3Int(1, 0, 0);


        if (tileBase == horizontalRoot)
        {
            if (direction == Direction.Down)
            {

            }
        }
    }

    List<Vector3Int> GetConnectedRoots(Vector3Int position)
    {
        List<Vector3Int> connectedRoots = new List<Vector3Int>();

        return connectedRoots;
    }

    public void Clip(Vector3Int position)
    {

    }

    public bool Extend(Vector3Int position, RootExtensionOp rootExtensionOp)
    {
        TileBase tileBase = rootTileMap.GetTile(position);

        if (rootExtensionOp == RootExtensionOp.CrossRoot)
        {
            // Check if valid position
            if (tileBase == verticalRoot ||
                tileBase == leftLRoot ||
                tileBase == rightLRoot)
            {
                // Add cross root
                rootTileMap.SetTile(position, crossRoot);
                return true;
            }
        }

        if (rootExtensionOp == RootExtensionOp.TRoot)
        {
            // Check if valid vertical position
            if (tileBase == verticalRoot)
            {
                // Add a random vertical T root
                if (Random.Range(0, 2) < 1)
                {
                    rootTileMap.SetTile(position, leftTRoot);
                }
                else
                {
                    rootTileMap.SetTile(position, righttTRoot);
                }
                return true;
            }

            // Check if valid horizontal position
            if (tileBase == horizontalRoot)
            {
                // Add horizontal T root
                rootTileMap.SetTile(position, downTRoot);
                growthPoints.Add(position);
                return true;
            }
        }

        if (rootExtensionOp == RootExtensionOp.HorizontalRoot)
        {
            if (tileBase == null)
            {
                Vector3Int endL = position + new Vector3Int(1, 0, 0);
                Vector3Int endR = position - new Vector3Int(1, 0, 0);

                TileBase tileBaseL = rootTileMap.GetTile(endL);
                TileBase tileBaseR = rootTileMap.GetTile(endR);

                if (tileBaseL == crossRoot || tileBaseL == leftTRoot ||
                    tileBaseR == crossRoot || tileBaseR == righttTRoot ||
                    tileBaseL == downTRoot || tileBaseR == downTRoot ||
                    tileBaseL == upTRoot || tileBaseR == upTRoot ||
                    tileBaseL == horizontalRoot || tileBaseR == horizontalRoot)
                {
                    rootTileMap.SetTile(position, horizontalRoot);
                    return true;
                }
                return false;
            }
        }

        return false;
    }*/

    public enum RootExtensionOp
    {
        TRoot,
        HorizontalRoot,
        CrossRoot
    }

    public enum Direction
    {
        Up,
        Down, 
        Left, 
        Right
    }

    [System.Serializable]
    struct Root
    {
        public Tile tile;
        public Direction[] contactPoints;
    }
}
