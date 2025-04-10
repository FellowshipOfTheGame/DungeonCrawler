using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class ReadRandomEnemyJSON : MonoBehaviour
{
    public TextAsset jsonData;  // Drag the JSON file into this field in the Unity editor

    private Dictionary<string, List<Enemy>> enemiesByFloor;
    private Dictionary<string, List<Encounter>> encountersByFloor;

    void Start()
    {
        // Deserialize JSON into a dictionary structure
        ParseJsonData(jsonData.ToString());
    }

    private void ParseJsonData(string json)
    {
        enemiesByFloor = new Dictionary<string, List<Enemy>>();
        encountersByFloor = new Dictionary<string, List<Encounter>>();

        // Parse floors, enemies, and encounters
        var jsonObject = JsonUtility.FromJson<JsonRoot>(json);

        foreach (var floorName in jsonObject.Floors.Keys)
        {
            enemiesByFloor[floorName] = new List<Enemy>();
            encountersByFloor[floorName] = new List<Encounter>();

            // Parse Enemies
            foreach (var enemy in jsonObject.Floors[floorName].Enemies)
            {
                
            }

            // Parse Encounters
            foreach (var encounter in jsonObject.Floors[floorName].Encounters)
            {
                var newEncounter = new Encounter
                {
                    Enemies = new List<string>(encounter.Enemies),
                    Chance = encounter.Chance
                };
                encountersByFloor[floorName].Add(newEncounter);
            }
        }
    }

    // Define structure for Enemy and Encounter
    [System.Serializable]
    public class Enemy
    {
        public string CharacterName;
        public int MaxHealth;
        public int MaxMana;
        public Attributes Attributes;
    }

    [System.Serializable]
    public class Encounter
    {
        public List<string> Enemies;
        public float Chance;
    }

    [System.Serializable]
    public class Attributes
    {
        public int strength;
        public int dexterity;
        public int intelligence;
        public int constitution;
        public int agility;
        public int knowledge;
    }

    [System.Serializable]
    public class JsonRoot
    {
        public Dictionary<string, FloorData> Floors;
    }

    [System.Serializable]
    public class FloorData
    {
        public List<Enemy> Enemies;
        public List<Encounter> Encounters;
    }
}
