using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootMouseManager : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;
    [SerializeField] RootManager rootManager;

    [Header("Audio")]
    [SerializeField] GameObject autoDieSFX;
    [SerializeField] List<AudioClip> removeRoot;
    [SerializeField] AudioClip purchase, failPurchase, fail;

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
            AudioClip clip = purchase;
            bool canPlace = false;
            if (currType != RootType.none)
            {
                if (currType == RootType.tU)
                {
                    canPlace = rootManager.CheckIfTRootPossible(tilePosition, RootManager.Direction.Up);
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Up);
                }
                else if (currType == RootType.tD)
                {
                    canPlace = rootManager.CheckIfTRootPossible(tilePosition, RootManager.Direction.Down);
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Down);
                }
                else if (currType == RootType.tL)
                {
                    canPlace = rootManager.CheckIfTRootPossible(tilePosition, RootManager.Direction.Left);
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Left);
                }
                else if (currType == RootType.tR)
                {
                    canPlace = rootManager.CheckIfTRootPossible(tilePosition, RootManager.Direction.Right);
                    rootManager.ChangeToTRoot(tilePosition, RootManager.Direction.Right);
                }
                else if (currType == RootType.lLU)
                {
                    canPlace = rootManager.CheckIfLRootPossible(tilePosition, RootManager.Direction.Left, RootManager.Direction.Up);
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Up);
                }
                else if (currType == RootType.lLD)
                {
                    canPlace = rootManager.CheckIfLRootPossible(tilePosition, RootManager.Direction.Left, RootManager.Direction.Down);
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Left, RootManager.Direction.Down);
                }
                else if (currType == RootType.lRU)
                {
                    canPlace = rootManager.CheckIfLRootPossible(tilePosition, RootManager.Direction.Right, RootManager.Direction.Up);
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Up);
                }
                else if (currType == RootType.lRD)
                {
                    canPlace = rootManager.CheckIfLRootPossible(tilePosition, RootManager.Direction.Right, RootManager.Direction.Down);
                    rootManager.AddLRoot(tilePosition, RootManager.Direction.Right, RootManager.Direction.Down);
                }
                else if (currType == RootType.hor)
                {
                    canPlace = rootManager.CheckIfHorizontalRootPossible(tilePosition);
                    rootManager.AddHorizontalRoot(tilePosition);
                }
                else if (currType == RootType.cross)
                {
                    canPlace = rootManager.CheckIfCrossRootPossible(tilePosition);
                    rootManager.SetToCrossRoot(tilePosition);
                }
                rootManager.EditMoisture(-currPrice);
                currType = RootType.none;
                currPrice = 0;
            }
            if (!canPlace) clip = fail;
            GameObject sfx = Instantiate(autoDieSFX);
            sfx.GetComponent<AudioAutoDie>().SetClip(clip);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (currType == RootType.none && rootManager.CanRemoveRoot(tilePosition))
            {
                rootManager.RemoveRoot(tilePosition);
                GameObject sfx = Instantiate(autoDieSFX);
                sfx.GetComponent<AudioAutoDie>().SetClip(removeRoot[Random.Range(0,removeRoot.Count)]);
            }
            currType = RootType.none;
            currPrice = 0;
        }
    }

    public void setRootType(int rt)
    {
        if (rootManager.CanPurchase(Prices[rt]))
        {
            currType = RootTypes[rt];
            currPrice = Prices[rt];
        }
        else
        {
            GameObject sfx = Instantiate(autoDieSFX);
            sfx.GetComponent<AudioAutoDie>().SetClip(failPurchase);
        }
    }
}
