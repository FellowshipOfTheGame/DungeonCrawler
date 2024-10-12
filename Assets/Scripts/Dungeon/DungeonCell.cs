public class DungeonCell
{
    public enum SpecialFeatureType
    {
        None,
        Entrance,
        Exit,
        Treasure,
        Trap,
    }

    public bool HasFloor { get; set; } = false;
    public bool WallNorth { get; set; } = false;
    public bool WallSouth { get; set; } = false;
    public bool WallEast { get; set; } = false;
    public bool WallWest { get; set; } = false;
    public bool HasSpecialFeature { get; set; } = false;
    public SpecialFeatureType SpecialFeature { get; set; } = SpecialFeatureType.None;

    public int SpecialFeatureValue { get; set; } = 0;
}
