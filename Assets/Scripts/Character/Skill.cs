using UnityEngine;
using System.Collections.Generic;

interface ISkillStrategy{
    public void DoSkill(Character user, Character target);
    public void DoSkill(Character user, List<Character> targets);
}

[CreateAssetMenu(fileName = "Skill", menuName = "Combat/Skill", order = 0)]
public class Skill: ScriptableObject{
    public string skillName;
    
    //TODO ADD ITEM REQUIREMENT
    [Range(0,99)]
    public int levelRequirement;
    
    [Range(0, 120)]
    public int manaCost;
    
    public enum TargetTeam
    {Friendly, Enemy}
    public TargetTeam target;
    
    public enum TargetType
    {Self, One, Multiple}
    public TargetType type;
    
    public enum SkillType
    {Damage, Heal, Stat}
    public SkillType skillStrategy;
    private ISkillStrategy _strategy;
}