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

    public enum RootType { none, hor, ver, tL, tR, tU, tD, lRD, lLD, lRU, lLU, cross };
    private static List<RootType> RootTypes = new List<RootType> { RootType.none, RootType.hor, RootType.ver, RootType.tL, RootType.tR, RootType.tU, RootType.tD, RootType.lRD, RootType.lLD, RootType.lRU, RootType.lLU, RootType.cross };
    private RootType currType;
    private static List<float> Prices = new List<float> { 0, 75f, 100f, 50f, 50f, 100f, 100f, 50f, 50f, 100f, 100f, 150f };
    private float currPrice;

    // Update is called once per frame
    void Update()
    {
        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePosition = tileMap.WorldToCell(worldPoint);
        if (Input.GetMouseButtonDown(0))
        {
            if (currType != RootType.none)
            {
                if (currType == RootType.tU)
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Up);
                }
                else if (currType == RootType.tD)
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Down);
                }
                else if (currType == RootType.tL)
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Left);
                }
                else if (currType == RootType.tR)
                {
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Right);
                }
                else if (currType == RootType.lLU)
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Up);
                }
                else if (currType == RootType.lLD)
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Down);
                }
                else if (currType == RootType.lRU)
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Up);
                }
                else if (currType == RootType.lRD)
                {
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Down);
                }
                else if (currType == RootType.hor)
                {
                    rootManager.AddHorizontalRoot(tilePosition);
                }
                else if (currType == RootType.cross)
                {
                    rootManager.SetToCrossRoot(tilePosition);
                }
                currType = RootType.none;
                rootManager.EditMoisture(-currPrice);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (currType == RootType.none) rootManager.RemoveRoot(tilePosition);
            currType = RootType.none;
        }
    }

    public void setRootType(int rt)
    {
        currType = RootTypes[rt];
        currPrice = Prices[rt];
    }
}
