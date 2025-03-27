using UnityEngine;

namespace DgTools
{
    public class ToggleStructure:EditorTool
    {
        public ToggleStructure(DungeonEditor dgEditor) : base(dgEditor){}

        public override void OnClickFloor(int x, int y)
        {
            DungeonEditor.Map[x, y].hasFloor = !DungeonEditor.Map[x, y].hasFloor;
            DungeonEditor.Grid[x, y].image.color = !DungeonEditor.Map[x, y].hasFloor ? Color.white : Color.gray;
        }

        public override void OnClickWall(int x, int y, DungeonTile.Side side)
        {
           DungeonEditor.Map[x, y].sides ^= side;
           DungeonEditor.WallGrid[x, y, DungeonEditor.SideToIdx[side]].image.color = 
               DungeonEditor.Map[x, y].sides.HasFlag(side) ? Color.white : Color.gray;
        }

        public override void OnEnter()
        {
            uint maxX = DungeonEditor.x, 
                maxY = DungeonEditor.y;
            
            for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxY; j++)
            {
                DungeonEditor.Grid[i, j].image.color = 
                        !DungeonEditor.Map[i, j].hasFloor ? Color.white : Color.gray;
                
                DungeonEditor.WallGrid[i, j, 0].image.color = 
                    DungeonEditor.Map[i, j].sides.HasFlag(DungeonTile.Side.North) ? Color.white : Color.gray;
                
               DungeonEditor.WallGrid[i, j, 1].image.color = 
                    DungeonEditor.Map[i, j].sides.HasFlag(DungeonTile.Side.East) ? Color.white : Color.gray;
                
               DungeonEditor.WallGrid[i, j, 2].image.color =
                    DungeonEditor.Map[i, j].sides.HasFlag(DungeonTile.Side.South) ? Color.white : Color.gray;
               
               DungeonEditor.WallGrid[i, j, 3].image.color =
                    DungeonEditor.Map[i, j].sides.HasFlag(DungeonTile.Side.West) ? Color.white : Color.gray;
            }
        }
        
        public override void OnExit()
        {}
    }
}