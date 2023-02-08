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
    [SerializeField] Tile horizontalLRoot;

    [SerializeField] List<Vector3Int> growthPoints = new List<Vector3Int>();


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
                newGrowthPoints.Add(point);
            }
        }

        growthPoints = newGrowthPoints;
    }

    public void Extend(Vector3Int position, bool toCross)
    {
        TileBase tileBase = rootTileMap.GetTile(position);

        if (toCross)
        {
            // Check if valid position
            if (tileBase == verticalRoot ||
                tileBase == leftLRoot ||
                tileBase == rightLRoot)
            {
                // Add cross root
                rootTileMap.SetTile(position, crossRoot);
            }

        }
        else
        {
            // Check if valid position
            if (tileBase == verticalRoot)
            {
                // Add a random vertical T root
                if (Random.Range(0, 1) == 0)
                {
                    rootTileMap.SetTile(position, leftLRoot);
                }
                else
                {
                    rootTileMap.SetTile(position, rightLRoot);
                }
            }
        }
    }
}
