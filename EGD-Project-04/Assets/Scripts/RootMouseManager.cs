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
    [SerializeField] List<AudioClip> plantRoot;
    [SerializeField] List<AudioClip> removeRoot;
    [SerializeField] AudioClip purchase, failPurchase, fail, cancel;

    [Header("Visual")]
    [SerializeField] List<Sprite> rootSprites;
    [SerializeField] Sprite clipper;
    [SerializeField] GameObject SelectedSquare, RootSprite;

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
    private bool remove = false;

    // Update is called once per frame
    void Update()
    {
        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(worldPoint);
        Vector3Int tilePosition = tileMap.WorldToCell(worldPoint);

        // visual
        if (currType != RootType.none || remove)
        {
            SelectedSquare.SetActive(true);
        }
        else
        {
            SelectedSquare.SetActive(false);
            RootSprite.GetComponent<SpriteRenderer>().sprite = rootSprites[0];
        }

        SelectedSquare.transform.position = RootSprite.transform.position = tilePosition + new Vector3(0.5f,0.5f,0);
        if (RootSprite.GetComponent<SpriteRenderer>().sprite == clipper) RootSprite.transform.position = tilePosition + new Vector3(1f, 0, 0);

        // actual placing or removing
        if (Input.GetMouseButtonDown(0))
        {
            AudioClip clip = plantRoot[Random.Range(0,plantRoot.Count)];
            bool canPlace = false;
            bool canRemove = worldPoint.y > -3f && worldPoint.x > 7f && worldPoint.x < -7f;
            if (currType != RootType.none && rootManager.CanPurchase(currPrice) && !remove)
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
            }
            else if (remove && canRemove)
            {
                clip = removeRoot[Random.Range(0, removeRoot.Count)];
                rootManager.RemoveRoot(tilePosition);
            }
            if (!canPlace) clip = fail;
            if (!rootManager.CanPurchase(currPrice)) clip = failPurchase;
            if (currType != RootType.none || (remove && canRemove))
            {
                GameObject sfx = Instantiate(autoDieSFX);
                sfx.GetComponent<AudioAutoDie>().SetClip(clip);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AudioClip clip = cancel;
            currType = RootType.none;
            currPrice = 0;
            remove = false;
            GameObject sfx = Instantiate(autoDieSFX);
            sfx.GetComponent<AudioAutoDie>().SetClip(clip);
        }
    }

    public void setRootType(int rt)
    {
        if (rootManager.CanPurchase(Prices[rt]))
        {
            remove = false;
            currType = RootTypes[rt];
            currPrice = Prices[rt];
            RootSprite.GetComponent<SpriteRenderer>().sprite = rootSprites[rt];
        }
        else
        {
            GameObject sfx = Instantiate(autoDieSFX);
            sfx.GetComponent<AudioAutoDie>().SetClip(failPurchase);
        }
    }
    public void setRemove() { remove = true; RootSprite.GetComponent<SpriteRenderer>().sprite = clipper; }
}
