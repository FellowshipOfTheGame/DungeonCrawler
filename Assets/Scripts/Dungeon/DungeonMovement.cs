using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonMovement : MonoBehaviour
{
    private enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    private DungeonGenerator dungeonGenerator;

    private Direction currentDirection = Direction.North;

    private DungeonCell currentCell;
    private int currentPosX;
    private int currentPosY;

    private DungeonCell nextCell;
    private int nextPosX;
    private int nextPosY;

    private bool canMove = false;

    [SerializeField] private float randomEncounterMaximumChance = 0.2f;
    [SerializeField] private float minStepsToEncounter = 5;
    [SerializeField] private float maxStepsToEncounter = 20;
    private float currentSteps = 0;
    private bool hasCheckedForEncounter = false;

    [SerializeField] private float playerHeight;
    [SerializeField] private int playerStartingPositionX;
    [SerializeField] private int playerStartingPositionY;

    [SerializeField] private float transitionSpeed;
    [SerializeField] private float transitionRotationSpeed;
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private bool isSpecialFeatureChecked = false;

    // Start is called before the first frame update
    void Start()
    {
        dungeonGenerator = FindFirstObjectByType<DungeonGenerator>();
        SetStartingPosition(playerStartingPositionX, playerStartingPositionY);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSpecialFeature();
        CheckForRandomEncounters();
        Movement();
    }

    public void SetStartingPosition(int x, int y)
    {
        currentCell = dungeonGenerator.GetCell(x, y);

        if (!currentCell.HasFloor)
        {
            print("invalid starting position");
            currentCell = null;
            return;
        }

        transform.position = new Vector3(x, playerHeight, y);
        targetPosition = transform.position;
        targetRotation = transform.eulerAngles;

        currentPosX = x;
        currentPosY = y;
    }

    private void CheckForSpecialFeature()
    {
        if (!canMove)
        {
            return;
        }

        if (isSpecialFeatureChecked)
        {
            return;
        }

        if (currentCell.HasSpecialFeature)
        {
            if (currentCell.SpecialFeature == DungeonCell.SpecialFeatureType.None)
            {
                return;
            }

            GameObject cell = GameObject.Find("Cell " + currentPosX + ", " + currentPosY);

            switch (currentCell.SpecialFeature)
            {
                case DungeonCell.SpecialFeatureType.Treasure:
                    print("Treasure found!");
                    currentCell.SpecialFeature = DungeonCell.SpecialFeatureType.None;
                    cell.GetComponentInChildren<Treasure>().AddTreasureToPlayer();
                    break;
                case DungeonCell.SpecialFeatureType.Trap:
                    print("Trap found!");
                    Trap trapComponent = cell.GetComponentInChildren<Trap>();
                    trapComponent.AddTrapToPlayer();
                    if (!trapComponent.isActiveAndEnabled)
                    {
                        print("Trap Destroyed!");
                        currentCell.SpecialFeature = DungeonCell.SpecialFeatureType.None;
                    }
                    break;
                case DungeonCell.SpecialFeatureType.StairDown:
                    print("Stair down");
                    // Get the map with Map id
                    int id = currentCell.SpecialFeatureValue;
                    string path = MapID.GetMapPath(id);
                    // Load the map
                    dungeonGenerator.UnloadDungeon();
                    dungeonGenerator.LoadDungeon(path);
                    break;
                case DungeonCell.SpecialFeatureType.StairUp:
                    print("Exit found!");
                    break;
                default:
                    print("Special feature not recognized");
                    break;
            }

            isSpecialFeatureChecked = true;
        }
    }

    private void CheckForRandomEncounters()
    {
        if (canMove || hasCheckedForEncounter)
        {
            return;
        }

        if (currentCell.HasSpecialFeature)
        {
            return;
        }

        hasCheckedForEncounter = true;
        currentSteps++;

        if (currentSteps <= minStepsToEncounter)
        {
            return;
        }

        float encounterChance;

        if (currentSteps >= maxStepsToEncounter)
        {
            encounterChance = randomEncounterMaximumChance;
        }
        else
        {
            encounterChance = randomEncounterMaximumChance * (currentSteps - minStepsToEncounter) / (maxStepsToEncounter - minStepsToEncounter);
        }

        if (Random.value < encounterChance)
        {
            print("Random encounter!");
            currentSteps = 0;
            SceneManager.LoadScene("Combat");
            // Start encounter
        }
    }

    public DungeonCell GetCellInFront(int currentPosX, int currentPosY)
    {
        int dislocationX = 0;

        if (currentDirection == Direction.East)
        {
            dislocationX = 1;
        }
        else if (currentDirection == Direction.West)
        {
            dislocationX = -1;
        }

        int dislocationY = 0;

        if (currentDirection == Direction.North)
        {
            dislocationY = 1;
        }
        else if (currentDirection == Direction.South)
        {
            dislocationY = -1;
        }

        nextPosX = currentPosX + dislocationX;
        nextPosY = currentPosY + dislocationY;
        DungeonCell dungeonCell = dungeonGenerator.GetCell(nextPosX, nextPosY);

        if (dungeonCell == null)
        {
            print("Next cell (" + currentPosX + ", " + currentPosY + ") is empty");
        }

        return dungeonGenerator.GetCell(nextPosX, nextPosY);
    }

    private bool CheckForWall()
    {
        if (currentDirection == Direction.North && currentCell.WallNorth)
        {
            print("Wall in north");
            return true;
        }
        else if (currentDirection == Direction.South && currentCell.WallSouth)
        {
            print("Wall in south");
            return true;
        }
        else if (currentDirection == Direction.East && currentCell.WallEast)
        {
            print("Wall in east");
            return true;
        }
        else if (currentDirection == Direction.West && currentCell.WallWest)
        {
            print("Wall in west");
            return true;
        }
        else
        {
            return false;
        }
    }

    bool isRotation = false;
    public void Movement()
    {
        MoveForwards();
        Rotate();

        if (Vector3.Distance(transform.position, targetPosition) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
            canMove = false;
            isRotation = false;
        }
        else if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.y)) > float.Epsilon)
        {
            Vector3 newRotation = new(0, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.y, transitionRotationSpeed * Time.deltaTime), 0);
            transform.eulerAngles = newRotation;
            canMove = false;
            isRotation = true;
        }
        else if (!canMove)
        {
            canMove = true;
            if (!isRotation){
                isSpecialFeatureChecked = false;
            }
        }

    }

    public void MoveForwards()
    {
        if (!canMove)
            return;

        if (!Input.GetKey(KeyCode.W))
            return;

        if (currentCell == null) // nao tem celula
        {
            print("Current cell is null");
            return;
        }

        if (CheckForWall()) // tem parede
        {
            return;
        }

        nextCell = GetCellInFront(currentPosX, currentPosY);

        if (nextCell == null) // fora do grid
        {
            print("Next cell is null");
            return;
        }

        if (!nextCell.HasFloor) // nao tem chao
        {
            print("Next cell has no floor");
            return;
        }

        targetPosition = new Vector3(nextPosX, playerHeight, nextPosY);
        currentCell = nextCell;
        currentPosX = nextPosX;
        currentPosY = nextPosY;

        hasCheckedForEncounter = false;

        print("Moved to " + nextPosX + ", " + nextPosY);
    }

    public void Rotate()
    {
        if (!canMove)
            return;

        bool rotated = false;
        float angle = transform.eulerAngles.y;

        if (Input.GetKey(KeyCode.A)) // Rotate left (counterclockwise)
        {
            angle -= 90f;
            currentDirection = (Direction)(((int)currentDirection + 3) % 4); // Update direction counterclockwise
            rotated = true;
        }
        else if (Input.GetKey(KeyCode.D)) // Rotate right (clockwise)
        {
            angle += 90f;
            currentDirection = (Direction)(((int)currentDirection + 1) % 4); // Update direction clockwise
            rotated = true;
        }

        if (rotated)
        {
            targetRotation = new Vector3(0, angle, 0);
        }
    }


}
