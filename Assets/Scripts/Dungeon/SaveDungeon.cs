using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class SaveDungeon : EditorWindow
{
    // Parameters
    private string savePath = "Assets/Scripts/Dungeon/example_dungeon.dat";
    
    [MenuItem("Save Dungeon/Save Dungeon")]
    public static void ShowWindow()
    {
        GetWindow<SaveDungeon>("Save Dungeon");
    }

    private void OnGUI()
    {
        GUILayout.Label("Parametros", EditorStyles.boldLabel);

        // Input fields
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        // Run Button
        if (GUILayout.Button("SalvarDungeon"))
        {

            DungeonCell[,] dungeon = new DungeonCell[10, 10]; // Exemplo de dungeon
            for(int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++)
                {
                    dungeon[x, y] = new DungeonCell();
                }
            }   
            dungeon[0, 0].HasFloor = true;
            dungeon[0, 0].WallNorth = true;
            dungeon[0, 0].WallWest = true;
            dungeon[0, 0].WallSouth = true;
            dungeon[1, 0].HasFloor = true;
            dungeon[1, 1].HasFloor = true;
            dungeon[2, 0].HasFloor = true;
            dungeon[2, 1].HasFloor = true;
            dungeon[2, 2].HasFloor = true;
            dungeon[2, 1].HasSpecialFeature = true;
            dungeon[2, 1].SpecialFeature = DungeonCell.SpecialFeatureType.Treasure;
            dungeon[2, 1].SpecialFeatureValue = 100;
            dungeon[2, 2].HasFloor = true;
            dungeon[2, 2].HasSpecialFeature = true;
            dungeon[2, 2].SpecialFeature = DungeonCell.SpecialFeatureType.Trap;
            dungeon[2, 2].SpecialFeatureValue = 10;
            dungeon[2, 1].SpecialFeatureValue = 100;
            dungeon[2, 3].HasFloor = true;
            dungeon[2, 3].HasSpecialFeature = true;
            dungeon[2, 3].SpecialFeature = DungeonCell.SpecialFeatureType.StairDown;
            dungeon[2, 3].SpecialFeatureValue = 1;

            SaveCurrentDungeon(savePath, dungeon);
        }
    }

    public static void SaveCurrentDungeon(string filePath, DungeonCell[,] dungeon)
    {
        BinaryFormatter formatter = new();
        FileStream fileStream = new(filePath, FileMode.Create); // Criar / Sobrescrever

        try
        {
            formatter.Serialize(fileStream, dungeon); // Serializa a dungeon
            Debug.Log("Dungeon saved to " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save dungeon: " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }
    }
}
