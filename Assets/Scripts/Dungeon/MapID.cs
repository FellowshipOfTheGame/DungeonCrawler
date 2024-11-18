using System.Collections.Generic;

public class MapID
{
    private static MapID instance;


    public static MapID Instance
    {
        get
        {
            instance ??= new MapID();

            return instance;
        }
    }


    private static Dictionary<int, string> mapID = new Dictionary<int, string>
    {
        { 0, "Assets/Scripts/Dungeon/dungeon.dat" },
        { 1, "Assets/Scripts/Dungeon/dungeon2.dat" }
    };

    public static string GetMapPath(int id)
    {
        if(mapID.ContainsKey(id))
            return mapID[id];

        return null;
    }
}
