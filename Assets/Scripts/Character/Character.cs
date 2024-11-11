using System;
using UnityEngine;

[System.Serializable] 
public struct Attributes:ICloneable
{
    public int strength;
    public int dexterity;
    public int intelligence;
        
    public int constitution;
    public int agility;
    public int knowledge;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

[Serializable]
public class Character
{
    public string CharacterName;
    
    public int Health, MaxHealth;
    public int Mana, MaxMana;

    public bool IsAlive => Health > 0; // TODO think about it
    
    public int Level;
    public void LevelUp() => throw new NotImplementedException();

    public CharacterClass CharacterClass;
    public int classPortraitIndex;
    
    public Attributes Attributes;
    
    public delegate void CharacterUpdate();
    public event CharacterUpdate OnChange;

    public Weapon weapon;
    public int BaseDamage => (weapon != null)?(Attributes.strength+weapon.damage):(Attributes.strength);
    public int BaseDefense => 0;
    
    
    private bool _actionAssigned = false;
    public bool IsActionAssigned
    {
        get => _actionAssigned;
        set {
            _actionAssigned = value;
            OnChange?.Invoke();
        }
    }
    private bool _isDefending = false;
    public bool IsDefending 
    { get => _isDefending;
        set { 
            _isDefending = value;
            OnChange?.Invoke();
        }
    }

    public string GetClassName() => CharacterClass.className;
    
    public bool initialized = false;
    public void Initialize()
    {
        MaxHealth = CharacterClass.baseHealth;
        Health = MaxHealth;
        MaxMana = CharacterClass.baseMana;
        Mana = MaxMana;

        Level = 1;
        
        Attributes = (Attributes)CharacterClass.baseAttribute.Clone();
        initialized = true;
    }

    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        Health = Math.Max(Health, 0);
        
        OnChange?.Invoke();
    }

    public Sprite GetImage() {
        return CharacterClass.Portraits[classPortraitIndex];
    }
}
