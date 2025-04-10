using UnityEngine;
using UnityEngine.UI;

public class DungeonUX : MonoBehaviour
{
    [SerializeField] private int dungeonWidth;
    [SerializeField] private int dungeonHeight;
    [SerializeField] private int numberOfFloors;
    [SerializeField] private Transform gridParent; // The parent that holds the grid

    private GridLayoutGroup gridLayoutGroup; // The GridLayoutGroup component
    private RectTransform gridRectTransform; // The RectTransform of the grid

    private DungeonCell[,,] floorGrid; // x = width, y = height, z = floor

    [Header("Cell sprites")]
    [SerializeField] private Sprite emptySquareImage; // Sprite for the square UI element
    [SerializeField] private Sprite floorImage; // Sprite for the square UI element
    [SerializeField] private Sprite wallImage; // Sprite for the square UI element
    [SerializeField] private Sprite specialFeatureImage; // Sprite for the square UI element

    void Start()
    {
        gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridRectTransform = gridParent.GetComponent<RectTransform>();
        GenerateGrid();
    }

    public void GenerateCell(int x, int y, int floor)
    {
        DungeonCell cell = floorGrid[x, y, floor];

        // Instantiate the new GameObject and set gridParent as the parent
        GameObject newCell = new($"Cell_{x}_{y}");
        newCell.transform.SetParent(gridParent);

        // Add RectTransform to the new cell (important for layout)
        _ = newCell.AddComponent<RectTransform>();

        Image cellImage = newCell.AddComponent<Image>();

        if (cell.HasFloor)
        {
            cellImage.sprite = floorImage;
        }
        else
        {
            cellImage.sprite = emptySquareImage;
        }

        // Optionally, set the name or tag for easier identification
        newCell.name = $"Cell_{x}_{y}";

        // Set the local scale to 1 (to prevent issues with scaling)
        newCell.transform.localScale = Vector3.one;
    }

    public void GenerateGrid()
    {
        // Clear any existing children
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // generate empty dungeon
        floorGrid = new DungeonCell[dungeonWidth, dungeonHeight, numberOfFloors];

        // Calculate the size of each cell to fit the grid within the screen size
        float gridWidth = gridRectTransform.rect.width; // Get the width of the grid area
        float gridHeight = gridRectTransform.rect.height; // Get the height of the grid area

        // Calculate cell size based on the grid dimensions and the number of cells
        float cellWidth = gridWidth / dungeonWidth;
        float cellHeight = gridHeight / dungeonHeight;

        // Set the cell size of the GridLayoutGroup
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Set the number of columns
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = dungeonWidth;

        // Generate the UI grid
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                floorGrid[x, y, 0] = new DungeonCell();
                floorGrid[0, 0, 0].HasFloor = true;
                GenerateCell(x, y, 0);
            }
        }

        // Force the layout to update after adding all the cells
        LayoutRebuilder.ForceRebuildLayoutImmediate(gridRectTransform);
    }

}
