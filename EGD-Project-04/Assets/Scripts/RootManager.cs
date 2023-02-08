using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] Tilemap rootTileMap;
    [SerializeField] Vector3Int originRootPosition = new Vector3Int(0, 4, 0);

    [Header("Roots")]
    [SerializeField] Tile originRoot;
    [SerializeField] Tile verticalRoot;
    [SerializeField] Tile leftTRoot;
    [SerializeField] Tile righttTRoot;
    [SerializeField] Tile leftLRoot;
    [SerializeField] Tile rightLRoot;
    [SerializeField] Tile crossRoot;
    [SerializeField] Tile horizontalRoot;
    [SerializeField] Tile horizontalTRoot;

    [SerializeField] List<Vector3Int> growthPoints = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> bendPoints = new List<Vector3Int>();


    // Start is called before the first frame update
    void Start()
    {
        // Set initial root position
        AddBaseRoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add Base Root")]
    void AddBaseRoot()
    {
        rootTileMap.SetTile(originRootPosition, originRoot);
        growthPoints.Add(originRootPosition);
    }

    [ContextMenu("Grow")]
    void Grow()
    {
        List<Vector3Int> newGrowthPoints = new List<Vector3Int>();

        foreach (Vector3Int point in growthPoints)
        {
            Vector3Int targetPoint = point - new Vector3Int(0, 1, 0);
            if (rootTileMap.GetTile(targetPoint) == null)
            {
                // populate
                newGrowthPoints.Add(targetPoint);
                rootTileMap.SetTile(targetPoint, verticalRoot);
            }
            else
            {
                // TODO: Check if this tile is a hazard
                newGrowthPoints.Add(point);
            }
        }

        growthPoints = newGrowthPoints;
    }

    void Bend()
    {
        List<Vector3Int> newBendPoints = new List<Vector3Int>();

        foreach (Vector3Int point in growthPoints)
        {
            Vector3Int endL = point + new Vector3Int(1, 0, 0);
            Vector3Int endR = point - new Vector3Int(1, 0, 0);

            TileBase tileBaseL = rootTileMap.GetTile(endL);
            TileBase tileBaseR = rootTileMap.GetTile(endR);

            // If either end is already occupied, continue

            // What kind of tile is this?

            /*Vector3Int targetPoint = point - new Vector3Int(0, 1, 0);
            if (rootTileMap.GetTile(targetPoint) == null)
            {
                // populate
                newBendPoints.Add(targetPoint);
                rootTileMap.SetTile(targetPoint, verticalRoot);
            }
            else
            {
                // TODO: Check if this tile is a hazard
                newBendPoints.Add(point);
            }*/
        }

        growthPoints = newBendPoints;
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
                rootTileMap.SetTile(position, horizontalTRoot);
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
                    tileBaseL == horizontalTRoot || tileBaseR == horizontalTRoot)
                {
                    rootTileMap.SetTile(position, horizontalRoot);
                    return true;
                }
                return false;
            }
        }

        return false;
    }

    public enum RootExtensionOp
    {
        TRoot,
        HorizontalRoot,
        CrossRoot
    }
}
