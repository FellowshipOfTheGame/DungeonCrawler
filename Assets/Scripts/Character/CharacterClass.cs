using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterClass", menuName = "RPG/Class", order = 0)]
public class CharacterClass : ScriptableObject
{
    public string className;
    public string classDescription;
    
    public int baseHealth, baseMana;
    public Attributes baseAttribute;
    
    public List<Skill> classSkills;
    public List<Sprite> Portraits;
}
