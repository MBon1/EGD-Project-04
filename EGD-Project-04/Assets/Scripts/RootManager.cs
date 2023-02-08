using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    [SerializeField] Tile baseRoot;
    [SerializeField] Vector3Int position = new Vector3Int(0, 4, 0);
    [SerializeField] Tilemap rootTileMap;

    //[ContextMenu("Paint")]
    void Paint()
    {
        rootTileMap.SetTile(position, baseRoot);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set initial root position
        rootTileMap.SetTile(position, baseRoot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
