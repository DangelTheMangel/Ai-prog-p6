using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class CityController : MonoBehaviour
{
    public Tilemap tilemap;
    public RuleTile roadTile, buildingTile, grassTile;
    public int cityWidth = 100;
    public int cityHeight = 100;
    public Vector2Int outsideCitySize;
    public int splits = 5;
    public int roadWidth = 3;
    public int minAreaSize = 6;
    public int lastChoice = -1;
    public Vector2Int minPoint, maxPoint;
    public MonsterAmount[] Villagers;
    public NavMeshSurface meshSurface;
    public GameObject baseObj, monsterController;
    public Transform villagerParent;
    public string villagerTag = "Villagers", areaParm = "walkingArea";
    public MonsterController monsterC;
    private string splittingAreasMessage = "Splitting the lands: {0}/{1}";
    private string fillingBuildingsMessage = "Constructing buildings: {0}/{1}";
    private string fillingRoadsMessage = "Paving roads: {0}/{1}";
    private string completionMessage = "The city is complete";
    string startingPouplating = "Starting populate the area";

    public void StartCreateCity(TMP_Text progressText, Slider progressSlider, GameObject lmenu, GameObject hud)
    {
        StartCoroutine(CreateCityCoroutine(progressText, progressSlider, lmenu, hud));
    }

    private IEnumerator CreateCityCoroutine(TMP_Text progressText, Slider progressSlider, GameObject lmenu, GameObject hud)
    {
        List<Area> areas = new List<Area> { new Area(0, 0, cityWidth, cityHeight, false) };

        for (int i = 0; i < splits; i++)
        {
            if (areas.Count == 0) break;

            int index = Random.Range(0, areas.Count);
            Area area = areas[index];
            areas.RemoveAt(index);
            splitArea(area, areas);

            // Update progress UI
            progressText.text = string.Format(splittingAreasMessage, i + 1, splits);
            progressSlider.value = (float)(i + 1) / splits;
            yield return null; // Yield control back to Unity to update the UI
        }

        for (int i = 0; i < areas.Count; i++)
        {
            fillArea(areas[i], buildingTile);

            // Update progress UI
            progressText.text = string.Format(fillingBuildingsMessage, i + 1, areas.Count);
            progressSlider.value = (float)(i + 1) / areas.Count;
            yield return null;
        }

        for (int i = 0; i < areas.Count; i++)
        {
            fillRoads(areas[i]);

            // Update progress UI
            progressText.text = string.Format(fillingRoadsMessage, i + 1, areas.Count);
            progressSlider.value = (float)(i + 1) / areas.Count;
            yield return null;
        }
        progressText.text = startingPouplating;
        fillLandscapeAndVillagers();

        // Final update
        progressText.text = completionMessage;
        progressSlider.value = 1.0f;
        lmenu.SetActive(false);
        hud.SetActive(true);
    }

    void splitArea(Area area, List<Area> areas)
    {
        if (area.width <= minAreaSize * 2 || area.height <= minAreaSize * 2)
        {
            areas.Add(area);
            return;
        }

        List<int> possibleChoices = new List<int> { 0, 1, 2 };
        int splitType = -1;
        if (lastChoice != -1)
        {
            possibleChoices.Remove(lastChoice);
        }
        splitType = possibleChoices[Random.Range(0, possibleChoices.Count)];
        lastChoice = splitType;

        if (splitType == 0) // Horizontal split
        {
            splitHorizontal(area, areas);
        }
        else if (splitType == 1) // Vertical split
        {
            splitVertical(area, areas);
        }
        else if (splitType == 2) // Diagonal split
        {
            splitDiagonal(area, areas);
        }
    }

    void splitHorizontal(Area area, List<Area> areas)
    {
        if (area.height <= minAreaSize * 2)
        {
            areas.Add(area);
            return;
        }

        int splitY = Random.Range(area.y + minAreaSize, area.y + area.height - minAreaSize);
        Area area1 = new Area(area.x, area.y, area.width, splitY - area.y, false);
        Area area2 = new Area(area.x, splitY, area.width, area.y + area.height - splitY, false);

        areas.Add(area1);
        areas.Add(area2);
    }

    void splitVertical(Area area, List<Area> areas)
    {
        if (area.width <= minAreaSize * 2)
        {
            areas.Add(area);
            return;
        }

        int splitX = Random.Range(area.x + minAreaSize, area.x + area.width - minAreaSize);
        Area area1 = new Area(area.x, area.y, splitX - area.x, area.height, false);
        Area area2 = new Area(splitX, area.y, area.x + area.width - splitX, area.height, false);

        areas.Add(area1);
        areas.Add(area2);
    }

    void splitDiagonal(Area area, List<Area> areas)
    {
        if (area.width <= minAreaSize * 2 || area.height <= minAreaSize * 2)
        {
            areas.Add(area);
            return;
        }

        bool isBottomLeft = Random.Range(0, 2) == 0;
        if (isBottomLeft)
        {
            Area area1 = new Area(area.x, area.y, area.width / 2, area.height / 2, true);
            Area area2 = new Area(area.x + area.width / 2, area.y + area.height / 2, area.width / 2, area.height / 2, true);
            areas.Add(area1);
            areas.Add(area2);
        }
        else
        {
            Area area1 = new Area(area.x + area.width / 2, area.y, area.width / 2, area.height / 2, true);
            Area area2 = new Area(area.x, area.y + area.height / 2, area.width / 2, area.height / 2, true);
            areas.Add(area1);
            areas.Add(area2);
        }
    }

    void fillArea(Area area, RuleTile tile)
    {
        for (int x = area.x; x < area.x + area.width; x++)
        {
            checkForX(x);
            for (int y = area.y; y < area.y + area.height; y++)
            {
                checkForY(y);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public void checkForX(int x)
    {
        if (x < minPoint.x)
        {
            minPoint.x = x;
        }
        else if (x > maxPoint.x)
        {
            maxPoint.x = x;
        }
    }

    public void checkForY(int y)
    {
        if (y < minPoint.y)
        {
            minPoint.y = y;
        }
        else if (y > maxPoint.y)
        {
            maxPoint.y = y;
        }
    }

    void fillRoads(Area area)
    {
        // Set roads along the top and bottom borders
        for (int x = area.x - roadWidth; x < area.x + area.width + roadWidth; x++)
        {
            for (int y = area.y - roadWidth; y < area.y; y++)
            {
                if (x >= 0 && y >= 0 && x < cityWidth && y < cityHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
            }
            for (int y = area.y + area.height; y < area.y + area.height + roadWidth; y++)
            {
                if (x >= 0 && y >= 0 && x < cityWidth && y < cityHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
            }
        }

        // Set roads along the left and right borders
        for (int y = area.y; y < area.y + area.height; y++)
        {
            for (int x = area.x - roadWidth; x < area.x; x++)
            {
                if (x >= 0 && y >= 0 && x < cityWidth && y < cityHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
            }
            for (int x = area.x + area.width; x < area.x + area.width + roadWidth; x++)
            {
                if (x >= 0 && y >= 0 && x < cityWidth && y < cityHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
            }
        }
    }


    public void fillLandscapeAndVillagers()
    {
        baseObj.SetActive(true);
        minPoint.x -= outsideCitySize.x;
        maxPoint.x += outsideCitySize.x;
        minPoint.y -= outsideCitySize.y;
        maxPoint.y += outsideCitySize.y;

        baseObj.transform.position = tilemap.CellToWorld(new Vector3Int((maxPoint.x + minPoint.x) / 2, (maxPoint.y + minPoint.y) / 2, 0));
        baseObj.transform.localScale = new Vector3(Mathf.Abs(minPoint.x) + maxPoint.x, 1, Mathf.Abs(minPoint.y) + maxPoint.y);

        List<Vector3Int> possibleSpawnPos = new List<Vector3Int>();

        for (int x = minPoint.x; x <= maxPoint.x; x++)
        {
            for (int y = minPoint.y; y <= maxPoint.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tilebase = tilemap.GetTile(pos);

                if (tilebase == null)
                {
                    tilemap.SetTile(pos, grassTile);
                }

                if (tilebase != buildingTile && x > 0 && x < cityWidth && y > 0 && y < cityHeight)
                {
                    possibleSpawnPos.Add(pos);
                }
            }
        }

        meshSurface.BuildNavMesh();

        foreach (MonsterAmount villager in Villagers)
        {
            for (int i = 0; i < villager.amount; i++)
            {
                if (possibleSpawnPos.Count == 0)
                {
                    Debug.LogWarning("Not enough spawn positions for all villagers");
                    Debug.Break();
                    return;
                }

                int index = Random.Range(0, possibleSpawnPos.Count);
                Vector3Int pos = possibleSpawnPos[index];
                Vector3 realPos = tilemap.CellToWorld(pos);
                GameObject instance = Instantiate(villager.prefab, new Vector3(realPos.x, 0, realPos.y), Quaternion.identity);
                possibleSpawnPos.RemoveAt(index);
                instance.GetComponent<BehaviorExecutor>().SetBehaviorParam(areaParm, baseObj);
                instance.transform.parent = villagerParent;
            }
        }

        if (possibleSpawnPos.Count > 0)
        {
            monsterController.transform.position = tilemap.CellToWorld(possibleSpawnPos[Random.Range(0, possibleSpawnPos.Count)]);
            monsterController.SetActive(true);
            monsterC.createMonster(possibleSpawnPos);
        }
        else
        {

            Debug.LogWarning("No spawn positions available for monster controller");
            Debug.Break();
        }
    }
}

    public struct Area
{
    public int x, y, width, height;
    public bool isTriangle;

    public Area(int x, int y, int width, int height, bool isTriangle)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.isTriangle = isTriangle;
    }
}
