using System;
using UnityEditorInternal;

[Flags]
public enum UsableWeapons
{
   None = 0,
   Sword = 1,
   Spear = 2,
   Shield = 4,
   Dagger = 8,
   Bow = 16,
   Crossbow = 32,
   Axe = 64
   //TODO ADD MORE WEAPON TYPES
}

public enum WeaponType
{
   None = 0,
   Sword = 1,
   Spear = 2,
   Shield = 4,
   Dagger = 8,
   Bow = 16,
   Crossbow = 32,
   Axe = 64
}

public class Weapon:Item
{
    public WeaponType weaponType;
    public Attributes weaponAttributes;
    public int damage;
   
}