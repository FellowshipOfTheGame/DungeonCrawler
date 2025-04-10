using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private GameObject stairDownPrefab;

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
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveDungeon("Assets/Scripts/Dungeon/dungeon.dat");
        }
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

            LoadDungeon("Assets/Scripts/Dungeon/dungeon2.dat");

            /*dungeon[0, 0].HasFloor = true;
            dungeon[0, 0].WallNorth = true;
            dungeon[0, 0].WallWest = true;
            dungeon[0, 0].WallSouth = true;
            dungeon[1, 0].HasFloor = true;
            dungeon[1, 1].HasFloor = true;
            dungeon[2, 0].HasFloor = true;
            dungeon[2, 1].HasFloor = true;
            dungeon[2, 2].HasFloor = true;
            dungeon[2, 1].HasSpecialFeature = true;
            dungeon[2, 1].SpecialFeature = DungeonCell.SpecialFeatureType.Treasure;
            dungeon[2, 1].SpecialFeatureValue = 100;
            dungeon[2, 2].HasFloor = true;
            dungeon[2, 2].HasSpecialFeature = true;
            dungeon[2, 2].SpecialFeature = DungeonCell.SpecialFeatureType.Trap;
            dungeon[2, 2].SpecialFeatureValue = 10;
            dungeon[2, 1].SpecialFeatureValue = 100;*/

            dungeon[1, 0].SpecialFeature = DungeonCell.SpecialFeatureType.StairDown;
            dungeon[1, 0].HasSpecialFeature = true;
            dungeon[1, 0].HasFloor = true;
            dungeon[1, 0].SpecialFeatureValue = 0;
        }
    }


    void GenerateFloors(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DungeonCell cell = dungeon[x, y];

                if (!cell.HasFloor)
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
                treasureComponent.SetValue(dungeon[x, y].SpecialFeatureValue);
                treasureComponent.treasureType = Treasure.TreasureType.Money;
                break;
            case DungeonCell.SpecialFeatureType.Trap:
                Instantiate(trapPrefab, position, rotation, generatedDungeonCell.transform);
                Trap trapComponent = trapPrefab.GetComponent<Trap>();
                trapComponent.SetValue(dungeon[x, y].SpecialFeatureValue);
                trapComponent.trapType = Trap.TrapType.Damage;
                break;
            case DungeonCell.SpecialFeatureType.StairUp:
                // Instantiate stair with the correct rotation
                break;
            case DungeonCell.SpecialFeatureType.StairDown:
                Instantiate(stairDownPrefab, position, rotation, generatedDungeonCell.transform);
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

    // Salva a dungeon em um arquivo binario
    public void SaveDungeon(string filePath)
    {
        BinaryFormatter formatter = new();
        FileStream fileStream = new(filePath, FileMode.Create); // Criar / Sobrescrever

        try
        {
            formatter.Serialize(fileStream, dungeon); // Serializa a dungeon
            Debug.Log("Dungeon saved to " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save dungeon: " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }
    }

    // Carrega a dungeon de um arquivo binario
    public void LoadDungeon(string filePath)
    {
        if (!File.Exists(filePath)) // Verifica se o arquivo existe
        {
            Debug.LogError("Dungeon file not found: " + filePath);
            return;
        }

        BinaryFormatter formatter = new();
        FileStream fileStream = new(filePath, FileMode.Open); // Abre o arquivo

        try
        {
            dungeon = (DungeonCell[,])formatter.Deserialize(fileStream);
            Debug.Log("Dungeon loaded from " + filePath);
            // Gera o Mapa
            GenerateFloors(width, height);
            GenerateWalls(width, height);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load dungeon: " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }
    }

    public void UnloadDungeon()
    {
        foreach (Transform child in dungeonParent.transform)
        {
            Destroy(child.gameObject);
        }

        dungeon = null;
    }
}
