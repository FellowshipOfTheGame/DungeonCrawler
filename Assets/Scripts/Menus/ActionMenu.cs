using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu:MonoBehaviour
{
   private Character character;
   private Combat combat;
   
   [SerializeField] private List<Button> actions;

   
   public void Start()
   {
      combat = Combat.GetInstance();
      Close();
   }

   public void Open(HeroPortrait heroPortrait)
   {
      character = heroPortrait.character;
      foreach (Button button in actions)
         button.interactable = true;
   }

   public void Close()
   {
      character = null;
      
      foreach (Button button in actions)
         button.interactable = false;
   }

   public void AttackAction()
   {
      combat.BeginAttackDeclaration(character);
   }

   public void DefendAction()
   {
      combat.DefendDeclaration(character);
   }

   public void OpenSkillMenu()
   {
      throw new NotImplementedException("SkillMenu Not Implemented");
   }

   public void EscapeAction()
   {
      throw new NotImplementedException("Escape Not Implemented");
   }
}