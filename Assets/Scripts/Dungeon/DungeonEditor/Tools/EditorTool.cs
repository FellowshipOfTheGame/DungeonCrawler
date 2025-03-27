namespace DgTools
{
    public abstract class EditorTool
    {
        protected DungeonEditor DungeonEditor;
        public EditorTool(DungeonEditor dgEditor)
        {
            DungeonEditor = dgEditor;
        }
        
        public abstract void OnClickFloor(int x, int y);
        public abstract void OnClickWall(int x, int y, DungeonTile.Side side);
        public abstract void OnEnter();
        public abstract void OnExit();    
    }
}