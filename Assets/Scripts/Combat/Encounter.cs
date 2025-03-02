using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Scriptable Objects/Encounter")]

public class Encounter : ScriptableObject
{
   public List<EncounterPosition> Formation;

   private void OnValidate()
   {
      if (Formation == null || Formation.Count == 0 || Formation.Count > 3)
         throw new Exception("Bad Encounter Formation. Must be between 0 and 3 enemies.");
      
      
      bool left = false,
         center = false,
         right = false;
      foreach (var e in Formation)
      {
         if(e.Enemy.IsUnityNull() || e.Enemy.characterClass == null)
            throw new Exception("Bad Encounter Formation. Enemy reference contains Null value.");

         if ((left && e.Position == EnemyPosition.Left) ||
             (right && e.Position == EnemyPosition.Right) ||
             (center && e.Position == EnemyPosition.Center))
            throw new Exception("Bad Encounter Formation. Multiple Enemies in same position.");

         switch (e.Position)
         {
            case EnemyPosition.Center:
               center = true;
               break;
            
            case EnemyPosition.Left:
               left = true;
               break;
            case EnemyPosition.Right: 
               right = true;
               break;
         }
      }
   }
}

[System.Serializable]
public class EncounterPosition
{
   public string EnemyTitle;
   public Character Enemy;
   public EnemyPosition Position;
}
