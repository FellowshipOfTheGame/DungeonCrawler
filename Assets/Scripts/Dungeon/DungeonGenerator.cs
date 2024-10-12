using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public int width = 10;
    public int height = 10;
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    private DungeonCell[,] dungeon;

    [SerializeField] private GameObject dungeonParent;

    [SerializeField] private GameObject treasurePrefab;

    // Start is called before the first frame update
    void Awake()
    {
        SetupDungeon();
        GenerateFloors(width, height);
        GenerateWalls(width, height);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Gera a dungeon a partir de uma matriz
     * (0, 0) = Canto Sudoeste
     */
    void SetupDungeon()
    {
        {
            DungeonCell[,] dungeonCells = new DungeonCell[width, height];
            dungeon = dungeonCells;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    dungeon[x, y] = new DungeonCell();
                    // Set walls and special features here
                }
            }

            dungeon[0, 0].HasFloor = true;
            dungeon[0, 0].WallNorth = true;
            dungeon[0, 0].WallWest = true;
            dungeon[0, 0].WallSouth = true;
            dungeon[1, 0].HasFloor = true;
            dungeon[2, 0].HasFloor = true;
            dungeon[1, 1].HasFloor = true;
            dungeon[2, 1].HasFloor = true;
            dungeon[2, 1].HasSpecialFeature = true;
            dungeon[2, 1].SpecialFeature = DungeonCell.SpecialFeatureType.Treasure;
            dungeon[2, 1].SpecialFeatureValue = 100;
        }
    }


    void GenerateFloors(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DungeonCell cell = dungeon[x, y];

                if(!cell.HasFloor)
                {
                    continue;
                }

                Vector3 position = new(x, 0, y); // Place at (x, 0, y)
                GameObject generatedCell = Instantiate(floorPrefab, position, Quaternion.identity, dungeonParent.transform);
                generatedCell.name = "Cell " + x + ", " + y;

                GenerateSpecialFeature(x, y, generatedCell);
            }
        }
    }

    void GenerateWalls(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DungeonCell cell = dungeon[x, y];

                Vector3 basePosition = new(x, 0, y);

                // North Wall
                if (cell.WallNorth)
                {
                    Vector3 northWallPos = basePosition + new Vector3(0, 0, 0.5f); // Adjust position
                    Instantiate(wallPrefab, northWallPos, Quaternion.Euler(0, 0, 0));
                }

                // South Wall
                if (cell.WallSouth)
                {
                    Vector3 southWallPos = basePosition + new Vector3(0, 0, -0.5f); // Adjust position
                    Instantiate(wallPrefab, southWallPos, Quaternion.Euler(0, 180, 0));
                }

                // East Wall
                if (cell.WallEast)
                {
                    Vector3 eastWallPos = basePosition + new Vector3(0.5f, 0, 0); // Adjust position
                    Instantiate(wallPrefab, eastWallPos, Quaternion.Euler(0, 90, 0));
                }

                // West Wall
                if (cell.WallWest)
                {
                    Vector3 westWallPos = basePosition + new Vector3(-0.5f, 0, 0); // Adjust position
                    Instantiate(wallPrefab, westWallPos, Quaternion.Euler(0, -90, 0));
                }
            }
        }
    }

    private void GenerateSpecialFeature(int x, int y, GameObject generatedDungeonCell)
    {
        if (!dungeon[x, y].HasSpecialFeature)
        {
            return;
        }
        print("entrou");

        Vector3 position = new(x, 0.05f, y);
        Quaternion rotation = Quaternion.identity; // Default rotation

        // Check which walls are present and rotate based on the side with no wall
        DungeonCell cell = dungeon[x, y];

        if (!cell.WallNorth)
        {
            // Rotate to face North
            rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!cell.WallSouth)
        {
            // Rotate to face South
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (!cell.WallEast)
        {
            // Rotate to face East
            rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (!cell.WallWest)
        {
            // Rotate to face West
            rotation = Quaternion.Euler(0, -90, 0);
        }

        // Instantiate the feature with the correct rotation
        switch (dungeon[x, y].SpecialFeature)
        {
            case DungeonCell.SpecialFeatureType.Treasure:
                Instantiate(treasurePrefab, position, rotation, generatedDungeonCell.transform);
                Treasure treasureComponent = treasurePrefab.GetComponent<Treasure>();
                treasureComponent.setValue(dungeon[x, y].SpecialFeatureValue);
                treasureComponent.treasureType = Treasure.TreasureType.Money;
                break;
            case DungeonCell.SpecialFeatureType.Trap:
                // Instantiate trap with the correct rotation
                break;
            case DungeonCell.SpecialFeatureType.Entrance:
                // Instantiate entrance with the correct rotation
                break;
            case DungeonCell.SpecialFeatureType.Exit:
                // Instantiate exit with the correct rotation
                break;
            default:
                break;
        }
    }


    public DungeonCell GetCell(int x, int y)
    {
        if (x < 0 || y < 0)
        { 
            print("Cell (" + x + ", " + y + ") is out of bounds");
            return null;
        }
        if (x >= width || y >= height)
        {
            print("Cell (" + x + ", " + y + ") is out of bounds");
            return null;
        }

        return dungeon[x, y];
    }
}
