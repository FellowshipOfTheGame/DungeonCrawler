using Unity.VisualScripting;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;

    public DungeonEditor dungeonEditor;

    private uint sizeX, sizeY;
    private DungeonTile[,] map;
    
    public void LoadFromDungeonEditor()
    {
        map = dungeonEditor.Map;
        sizeX = dungeonEditor.x;
        sizeY = dungeonEditor.y; 
        
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
                SetupCell(map[i,j],i, j);
        }
    }

    void SetupCell(DungeonTile tile, int x, int y)
    {
        if (tile.IsUnityNull()) return;
        GameObject cellObject = Instantiate(new GameObject(), new Vector3(x, 0, y), Quaternion.identity);
        
        //setup floor
        if (tile.hasFloor)
        {
            var cellFloor = Instantiate(floor, new Vector3(x, 0, y), Quaternion.identity,cellObject.transform);
            cellObject.transform.SetParent(transform);
        }
        
        //setup walls
        if (tile.sides.HasFlag(DungeonTile.Side.North))
            Instantiate(wall, new Vector3(x, 0, y+.5f),
                Quaternion.identity, cellObject.transform);
        
        if (tile.sides.HasFlag(DungeonTile.Side.East))
            Instantiate(wall, new Vector3(x+.5f, 0, y),
                Quaternion.Euler(0,90,0), cellObject.transform);
        
        if (tile.sides.HasFlag(DungeonTile.Side.South))
            Instantiate(wall, new Vector3(x, 0, y-.5f),
                Quaternion.identity,cellObject.transform);
        
        if (tile.sides.HasFlag(DungeonTile.Side.West))
            Instantiate(wall, new Vector3(x-.5f, 0, y),
                Quaternion.Euler(0,90,0), cellObject.transform);
    }

    public DungeonTile GetCell(int x, int y)
    {
        return map[x,y];
    }

    public (uint,uint) GetDimensions()
    {
        return (sizeX, sizeY);
    }
}
