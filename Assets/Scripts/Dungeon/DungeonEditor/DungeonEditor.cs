using System;
using System.Collections.Generic;
using System.Linq;
using DgTools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DungeonEditor : MonoBehaviour
{
    public GameObject buttonPref;
    public uint x = 6, y = 6;
    
    public DungeonTile[,] Map;
    
    public Button[,] Grid; 
    public Button[,,] WallGrid;
    
    public int floorButtonSize = 50;
    public int floorButtonGap = 50;

    public List<EditorTool> Tools;
    public EditorTool CurrentTool;
    
    //Dictionary to convert Side to WallGrid index, Side is a flag enum, so it's a bit paia
    public static Dictionary<DungeonTile.Side, int> SideToIdx = new Dictionary<DungeonTile.Side, int>
    {
        {DungeonTile.Side.North, 0},
        {DungeonTile.Side.East, 1},
        {DungeonTile.Side.South, 2},
        {DungeonTile.Side.West, 3},
    }; 
    
    public void Start()
    {
        BuildUI();
        
        Tools = new List<EditorTool>
        {
            new ToggleStructure(this),
        };
        SetTool(0);
    }

    void BuildUI()
    {
        Grid = new Button[x, y];
        WallGrid = new Button[x, y, 4];
        Map = new DungeonTile[x, y];

        for (int i = 0; i < x; i++)
        for (int j = 0; j < y; j++)
            BuildTile(i, j);    
    }
    
    public void SetTool(int toolIdx)
    { 
        CurrentTool?.OnExit(); 
        CurrentTool = Tools[toolIdx];
        CurrentTool.OnEnter();
    }
    
    private void BuildTile(int i, int j)
    {
        Vector2 panelSize = new Vector2(floorButtonSize*x + floorButtonGap*(+1), 
            floorButtonSize*y + floorButtonGap*(y+1));
        Map[i,j] = new DungeonTile();
        
        //Floor button and logic
        var button = GenerateFloorButton(i, j, panelSize).GetComponent<Button>();
        button.onClick.AddListener(GenerateFloorClojure(i,j));
        Grid[i,j] = button;
        
        //Wall Buttons and Logic
        foreach (var side in Enum.GetValues(typeof(DungeonTile.Side)).Cast<DungeonTile.Side>())
        {
            var wallButton = GenerateWallButton(i,j,side);
            wallButton.GetComponent<Button>().
                onClick.AddListener(GenerateWallClojure(i,j,side));
        }
    }

    private GameObject GenerateFloorButton(int i, int j, Vector2 panelsize)
    {
        var button = Instantiate(buttonPref,transform);
        Grid[i, j] = button.GetComponent<Button>();
        button.name = $"FloorButton{i},{j}";
        
        var rectT = button.GetComponent<RectTransform>();
        rectT.localPosition = new Vector3(i * floorButtonSize + i*floorButtonGap - panelsize.x / 2, 
            j * floorButtonSize + j*floorButtonGap - panelsize.y/2);    
        rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonSize);
        
        return button;
    }
    
    Button GenerateWallButton(int i, int j, DungeonTile.Side side)
    {
        var button = Instantiate(buttonPref, Grid[i,j].transform);
        WallGrid[i, j,SideToIdx[side]] = button.GetComponent<Button>();
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

    UnityAction GenerateFloorClojure(int i, int j)
    {
        return () =>
            CurrentTool.OnClickFloor(i,j);
    }
    
    UnityAction GenerateWallClojure(int i, int j, DungeonTile.Side side)
    {
        return () =>
            CurrentTool.OnClickWall(i,j,side);
    }
}