using System;
using System.Collections.Generic;

[System.Serializable]
public class DungeonTile
{
    [Flags] //Pra ficar bonito no inspector da unity
    public enum Side {
        North = 1, East = 1 << 1, South = 1 << 2, West = 1 << 3
    }
    public bool hasFloor = false;
    
    public Side sides = Side.North | Side.East | Side.South | Side.West;
    
    public List<TileFeature> Features;
    
    public Action<PlayerSave> OnEnter;
    public Action<PlayerSave> OnExit;
}

