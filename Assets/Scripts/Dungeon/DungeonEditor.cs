using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DungeonEditor : MonoBehaviour
{
    public GameObject buttonPref;
    public uint x = 6, y = 6;
    public DungeonTile[,] Map;
    
    private GameObject[,] grid;
    
    public int floorButtonSize = 50;
    public int floorButtonGap = 50;
    public void Start() => BuildUI();

    void BuildUI()
    {
        grid = new GameObject[x, y];
        Map = new DungeonTile[x, y];

        for (int i = 0; i < x; i++)
        for (int j = 0; j < y; j++)
            BuildTile(i, j);    
    }
    
    private GameObject BuildTile(int i, int j)
    {
        Vector2 panelSize = new Vector2(floorButtonSize*x + floorButtonGap*(+1), 
            floorButtonSize*y + floorButtonGap*(y+1));
        Map[i,j] = new DungeonTile();
        
        //Floor button and logic
        var button = GenerateFloorButton(i, j, panelSize);
        button.GetComponent<Button>().onClick.AddListener(GenerateFloorClojure(i,j));
        grid[i,j] = button;
        
        //Wall Buttons and Logic
        foreach (var side in Enum.GetValues(typeof(DungeonTile.Side)).Cast<DungeonTile.Side>())
        {
            var wallButton = GenerateWallButton(i,j,side);
            wallButton.GetComponent<Button>().
                onClick.AddListener(GenerateWallClojure(i,j,side,wallButton));
        }
        
        return button;
    }

    private GameObject GenerateFloorButton(int i, int j, Vector2 panelsize)
    {
        var button = Instantiate(buttonPref,transform);
        button.name = $"FloorButton{i},{j}";
        
        var rectT = button.GetComponent<RectTransform>();
        rectT.localPosition = new Vector3(i * floorButtonSize + i*floorButtonGap - panelsize.x / 2, 
            j * floorButtonSize + j*floorButtonGap - panelsize.y/2);    
        rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonSize);
        
        grid[i, j] = button;
        return button;
    }

    UnityAction GenerateFloorClojure(int i, int j)
    {
        var bColor = grid[i, j].GetComponent<Image>();
        return () =>
        {
            Map[i, j].hasFloor = !Map[i, j].hasFloor;
            bColor.color =  !Map[i,j].hasFloor? Color.white : Color.gray; 
        };
    }

    Button GenerateWallButton(int i, int j, DungeonTile.Side side)
    {
        var button = Instantiate(buttonPref, grid[i,j].transform);
        button.name = $"WallButton({i},{j}-{side})";
        
        var rectT = button.GetComponent<RectTransform>();
        switch (side){
            case DungeonTile.Side.North:
                rectT.localPosition = Vector3.up * floorButtonSize/2;
                break;
            case DungeonTile.Side.South:
                rectT.localPosition = Vector3.down * floorButtonSize/2;
                break;
            case DungeonTile.Side.East:
                rectT.localPosition = Vector3.right * floorButtonSize/2;
                break;
            case DungeonTile.Side.West:
                rectT.localPosition = Vector3.left * floorButtonSize/2;
                break;
        }
        
        if(side == DungeonTile.Side.North || side == DungeonTile.Side.South)
            rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonGap);
        else
            rectT.sizeDelta = new Vector2(floorButtonGap,floorButtonSize);
        
        return button.GetComponent<Button>();
    }

    UnityAction GenerateWallClojure(int i, int j, DungeonTile.Side side, Button WallButton)
    {
        var bColor = WallButton.image;
        return () =>
        {
            Map[i, j].sides ^= side;
            bColor.color =  Map[i, j].sides.HasFlag(side)? Color.white : Color.gray;
        };
    }

}