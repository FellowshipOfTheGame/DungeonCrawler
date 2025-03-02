using System;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEditor : MonoBehaviour
{
    public GameObject buttonPref;
    public uint x = 6, y = 6;
    private GameObject[,] Grid;
    public DungeonTile[,] map;
    
    public int floorButtonSize = 50;
    public int floorButtonGap = 50;
    public void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        Vector2 panelsize = new Vector2(floorButtonSize*x + floorButtonGap*(+1), 
            floorButtonSize*y + floorButtonGap*(y+1));
        
        Grid = new GameObject[x, y];
        map = new DungeonTile[x, y];
        
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                map[i,j] = new DungeonTile();
                var button = Instantiate(buttonPref,transform);
                Grid[i, j] = button;
                
                var rectT = button.GetComponent<RectTransform>();
                rectT.localPosition = new Vector3(i * floorButtonSize + i*floorButtonGap - panelsize.x / 2, 
                    j * floorButtonSize + j*floorButtonGap - panelsize.y/2);    
                rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonSize);

                int i1 = i, j1 = j;
                var b_color = Grid[i1, j1].GetComponent<Image>();
                Action closure = () =>
                {
                    map[i1, j1].hasFloor = !map[i1, j1].hasFloor;
                    b_color.color =  !map[i1,j1].hasFloor? Color.white : Color.gray; 
                };
                
                button.GetComponent<Button>().onClick.AddListener(() => closure());
            }
        }
        
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                //North
                var button = Instantiate(buttonPref, Grid[i,j].transform);
                var rectT = button.GetComponent<RectTransform>();
                rectT.localPosition = new Vector3(0,floorButtonSize/2);    
                rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonGap);
             
                int i1 = i, j1 = j;
                var bcolorN = button.GetComponent<Image>();

                void ClosureN()
                {
                    map[i1, j1].sides ^= DungeonTile.Side.North;
                    bcolorN.color = map[i1, j1].sides.HasFlag(DungeonTile.Side.North) ? Color.white : Color.gray;
                }

                button.GetComponent<Button>().onClick.AddListener(ClosureN);
                
                //East
                button = Instantiate(buttonPref, Grid[i,j].transform);
                rectT = button.GetComponent<RectTransform>();
                rectT.localPosition = new Vector3(floorButtonSize/2,0);    
                rectT.sizeDelta = new Vector2(floorButtonGap, floorButtonSize);
                
                var bcolorE = button.GetComponent<Image>();

                void ClosureE()
                {
                    map[i1, j1].sides ^= DungeonTile.Side.East;
                    bcolorE.color = map[i1, j1].sides.HasFlag(DungeonTile.Side.East) ? Color.white : Color.gray;
                }

                button.GetComponent<Button>().onClick.AddListener(ClosureE);
                
                //South
                button = Instantiate(buttonPref, Grid[i,j].transform);
                rectT = button.GetComponent<RectTransform>();
                rectT.localPosition = new Vector3(0,-floorButtonSize/2);
                rectT.sizeDelta = new Vector2(floorButtonSize, floorButtonGap);
                
                var bcolorS = button.GetComponent<Image>();

                void ClosureS()
                {
                    map[i1, j1].sides ^= DungeonTile.Side.South;
                    bcolorS.color = map[i1, j1].sides.HasFlag(DungeonTile.Side.South) ? Color.white : Color.gray;
                }

                button.GetComponent<Button>().onClick.AddListener(ClosureS);
                
                //West
                button = Instantiate(buttonPref, Grid[i,j].transform);
                rectT = button.GetComponent<RectTransform>();
                rectT.localPosition = new Vector3(-floorButtonSize/2,0);
                rectT.sizeDelta = new Vector2(floorButtonGap, floorButtonSize);
                
                var bcolorW = button.GetComponent<Image>();

                void ClosureW()
                {
                    map[i1, j1].sides ^= DungeonTile.Side.West;
                    bcolorW.color = map[i1, j1].sides.HasFlag(DungeonTile.Side.West) ? Color.white : Color.gray;
                }

                button.GetComponent<Button>().onClick.AddListener(ClosureW);
            }
        }
    }
}
