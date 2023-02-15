using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] Tilemap rootTileMap;
    [SerializeField] Vector3Int originRootPosition = new Vector3Int(0, 4, 0);
    [Space(10)]
    [SerializeField] float growthSpeed = 0.5f;
    [SerializeField] float delayedGrowthDuration = 2;
    [SerializeField] float collectionSpeed = 0.1f;
    [SerializeField] float collectionAmount = 5.5f;
    private float moisture = 0;
    HashSet<Vector3Int> searchedPoints = new HashSet<Vector3Int>();
    [SerializeField] Text moistureUI;

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
    [SerializeField] List<Vector3Int> clippedPoints = new List<Vector3Int>();
    [SerializeField] public Vector3Int lowestRootPosition { get; private set; } = Vector3Int.zero;



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

        // Start Moisture Collection
        StartCoroutine(CollectMoisture());

        // Start Root Growth
        StartCoroutine(RootGrowth());
    }

    private void Update()
    {
        moistureUI.text = "" + moisture;
    }

    IEnumerator RootGrowth()
    {
        yield return new WaitForSeconds(growthSpeed);
        //Debug.Log(searchedPoints.Count);
        Grow();
        StartCoroutine(RootGrowth());
    }
    IEnumerator CollectMoisture()
    {
        yield return new WaitForSeconds(collectionSpeed);
        int numRoots = CountBranchesOffOrigin();
        float moistureGains = numRoots * collectionAmount;
        moisture += moistureGains;
        //Debug.Log(numRoots + " yielded " + moistureGains + " moisture");
        StartCoroutine(CollectMoisture());
    }

    IEnumerator DelayGrowth(Vector3Int point)
    {
        yield return new WaitForSeconds(delayedGrowthDuration);
        if (clippedPoints.Contains(point))
        {
            clippedPoints.Remove(point);
        }
    }

    Direction InvertDirection(Direction direction)
    {
        if (direction == Direction.Up)
        {
            return Direction.Down;
        }
        else if (direction == Direction.Down)
        {
            return Direction.Up;
        }
        else if (direction == Direction.Left)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.Left;
        }
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

        // Check if the root's position is in the clippedPoints list
        if (clippedPoints.Contains(position))
        {
            StopCoroutine(DelayGrowth(position));
            clippedPoints.Remove(position);
        }

        // Check if root's position is the deepest root in map
        if (position.y < lowestRootPosition.y)
        {
            lowestRootPosition = position;
        }
    }

    public bool CanRemoveRoot(Vector3Int position)
    {
        Tile root = rootTileMap.GetTile<Tile>(position);

        if (root == null || root == originRoot.tile)
        {
            return false;
        }

        return true;
    }

    public void RemoveRoot(Vector3Int position)
    {
        Tile root = rootTileMap.GetTile<Tile>(position);

        if (!CanRemoveRoot(position))
        {
            return;
        }

        rootTileMap.SetTile(position, null);

        if (growthPoints.Contains(position))
        {
            growthPoints.Remove(position);
        }

        Vector3Int abovePosition = ContactPointToPosition(position, Direction.Up);
        root = rootTileMap.GetTile<Tile>(abovePosition);
        if (root != null && HasContactPoint(rootsByTile[root].contactPoints, Direction.Down) && 
            !growthPoints.Contains(abovePosition))
        {
            growthPoints.Add(abovePosition);

            if (!clippedPoints.Contains(position))
            {
                clippedPoints.Add(position);
                StartCoroutine(DelayGrowth(position));
            }
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
            // Check if root is in searchedPoints (if it's connected to main root)
            // and the tile below is empty
            Vector3Int targetPoint = ContactPointToPosition(point, Direction.Down);
            if (searchedPoints.Contains(point) && 
                rootTileMap.GetTile(targetPoint) == null && 
                !clippedPoints.Contains(targetPoint))
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

    public bool CheckIfLRootPossible(Vector3Int position, Direction horizontalDirection, Direction verticalDirection)
    {
        if ((horizontalDirection != Direction.Left && horizontalDirection != Direction.Right) ||
            (verticalDirection != Direction.Up && verticalDirection != Direction.Down))
        {
            Debug.LogError("INVALID DIRECTION");
            return false;
        }

        Tile tile = rootTileMap.GetTile<Tile>(position);
        if (tile != null)
        {
            return false;
        }

        // Get tiles L root could connect to
        Tile horizontalTile = rootTileMap.GetTile<Tile>(ContactPointToPosition(position, InvertDirection(horizontalDirection)));
        Tile verticalTile = rootTileMap.GetTile<Tile>(ContactPointToPosition(position, verticalDirection));

        if (horizontalTile != null && HasContactPoint(rootsByTile[horizontalTile].contactPoints, horizontalDirection) || 
            verticalTile != null && HasContactPoint(rootsByTile[verticalTile].contactPoints, InvertDirection(verticalDirection)))
        {
            return true;
        }

        return false;
    }

    public void AddLRoot(Vector3Int position, Direction horizontalDirection, Direction verticalDirection)
    {
        if ((horizontalDirection != Direction.Left && horizontalDirection != Direction.Right) ||
            (verticalDirection != Direction.Up && verticalDirection != Direction.Down))
        {
            Debug.LogError("INVALID DIRECTION");
            return;
        }

        if (!CheckIfLRootPossible(position, horizontalDirection, verticalDirection))
        {
            Debug.Log("Failed L Root");
            return;
        }

        if (verticalDirection == Direction.Up && horizontalDirection == Direction.Left)
        {
            Debug.Log("Adding UL L Root");
            AddRoot(leftUpLRoot, position);
        }
        else if (verticalDirection == Direction.Down && horizontalDirection == Direction.Left)
        {
            Debug.Log("Adding DL L Root");
            AddRoot(leftDownLRoot, position);
        }
        else if(verticalDirection == Direction.Up && horizontalDirection == Direction.Right)
        {
            Debug.Log("Adding UR L Root");
            AddRoot(rightUpLRoot, position);
        }
        else if (verticalDirection == Direction.Down && horizontalDirection == Direction.Right)
        {
            Debug.Log("Adding DR L Root");
            AddRoot(rightDownLRoot, position);
        }
    }

    public bool CheckIfHorizontalRootPossible(Vector3Int position)
    {
        Tile tile = rootTileMap.GetTile<Tile>(position);
        if (tile != null)
        {
            return false;
        }

        foreach(Direction d in horizontalRoot.contactPoints)
        {
            Tile t = rootTileMap.GetTile<Tile>(ContactPointToPosition(position, d));
            if (t != null && HasContactPoint(rootsByTile[t].contactPoints, InvertDirection(d)))
            {
                return true;
            }
        }

        return false;
    }

    public void AddHorizontalRoot(Vector3Int position)
    {
        if (CheckIfHorizontalRootPossible(position))
        {
            AddRoot(horizontalRoot, position);
        }
    }

    public bool CheckIfCrossRootPossible(Vector3Int position)
    {
        Tile tile = rootTileMap.GetTile<Tile>(position);
        if (tile != null)
        {
            return true;
        }

        foreach (Direction d in crossRoot.contactPoints)
        {
            Tile t = rootTileMap.GetTile<Tile>(ContactPointToPosition(position, d));
            if (t != null && HasContactPoint(rootsByTile[t].contactPoints, InvertDirection(d)))
            {
                return true;
            }
        }

        return false;
    }

    public void SetToCrossRoot(Vector3Int position)
    {
        if (CheckIfCrossRootPossible(position))
        {
            if (rootTileMap.GetTile<Tile>(position) != null)
            {
                RemoveRoot(position);
            }
            AddRoot(crossRoot, position);
        }
    }

    int CountBranchesOffOrigin()
    {
        searchedPoints.Clear();
        TraverseRootSystem(originRootPosition);
        return searchedPoints.Count;
    }

    void TraverseRootSystem(Vector3Int point)
    {
        if (searchedPoints.Contains(point))
        {
            return;
        }

        Tile rootTile = rootTileMap.GetTile<Tile>(point);
        if (rootTile == null)
        {
            return;
        }

        searchedPoints.Add(point);
        Root root = rootsByTile[rootTile];

        foreach(Direction dir in root.contactPoints)
        {
            Vector3Int newPoint = ContactPointToPosition(point, dir);
            // Check if there is a root on this end that can connect to this root
            Tile newRootTile = rootTileMap.GetTile<Tile>(newPoint);
            if (newRootTile != null &&
                HasContactPoint(rootsByTile[newRootTile].contactPoints, InvertDirection(dir)))
            {
                TraverseRootSystem(newPoint);
            }
        }
    }

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

    public void EditMoisture(float amt)
    {
        moisture += amt;
    }
    public bool CanPurchase(float amt)
    {
        return moisture - amt >= 0;
    }
}
