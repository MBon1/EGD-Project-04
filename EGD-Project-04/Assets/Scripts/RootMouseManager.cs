using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootMouseManager : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;
    [SerializeField] RootManager rootManager;

    [Header("Testing")]
    public int level1Fertilizer = 0;
    public int level2Fertilizer = 0;
    public int level3Fertilizer = 0;

    Vector2 worldPoint;
    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePosition = tileMap.WorldToCell(worldPoint);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                //if (level1Fertilizer > 0)
                {
                    if (rootManager.Extend(tilePosition, RootManager.RootExtensionOp.TRoot))
                    {
                        level1Fertilizer--;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                //if (level2Fertilizer > 0)
                {
                    if (rootManager.Extend(tilePosition, RootManager.RootExtensionOp.HorizontalRoot))
                    {
                        level2Fertilizer--;
                    }
                }
            }
            else
            {
                //if (level3Fertilizer > 0)
                {
                    if (rootManager.Extend(tilePosition, RootManager.RootExtensionOp.CrossRoot))
                    {
                        level2Fertilizer--;
                    }
                }
            }
        }
    }
}
