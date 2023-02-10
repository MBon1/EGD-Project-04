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

    public int seeds = 0;

    Vector2 worldPoint;
    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {*/
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePosition = tileMap.WorldToCell(worldPoint);

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Down);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Right);
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Down);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Down);
                }
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                rootManager.AddHorizontalRoot(tilePosition);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                rootManager.SetToCrossRoot(tilePosition);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                rootManager.RemoveRoot(tilePosition);
            }
        //}
    }
}
