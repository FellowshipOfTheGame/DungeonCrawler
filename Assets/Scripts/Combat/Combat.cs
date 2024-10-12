using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatAction;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private static Combat _instance;
    public PlayerSave save;
    public List<Character> Heroes;
    public List<Character> Enemies;

    public List<testCharacterLoader> heroPortraits;
    
    public void OnEnable()
    {
        _instance = this;
        Heroes = save.GetHeroes();
        for (int i = 0; i < Heroes.Count; i++)
            heroPortraits[i].Setup(Heroes[i]);

        StartCoroutine(TurnCoroutine());
    }
    
    private IEnumerator TurnCoroutine(){
        while (true) // Set ExitCondition
        {
            //Wait for all Heroes to take action
            yield return new WaitWhile
            (() => 
                Heroes.Count(c => c.IsAlive)-1 > CharacterActions.Count()
            );
            
            //TODO Enemy Actions :D
            
            
            ExecuteActions();
        }
    }

    public static Combat GetInstance()
    {
        if (_instance == null) throw new Exception("Uninstantiated combat reference.");
        return _instance;
    }

    public int Attack(Character attacker, Character target)
    {
        //TODO Change for something interesting
        int damage = attacker.BaseDamage;
        int defense = target.BaseDefense;
        int dealt_damage = Math.Max(attacker.BaseDamage - target.BaseDefense, 0);
        target.ReceiveDamage(dealt_damage);
        return dealt_damage;
    }
    
    //Todo Rename to toggle
    public void OpenActionMenu(testCharacterLoader target)
    {
        CloseActionMenus();
        target.ActionMenu.SetActive(!target.ActionMenu.activeSelf);
    }

    public void CloseActionMenus()
    {
        heroPortraits.ForEach(p =>p.ActionMenu.SetActive(false));
    }

    #region ActionDeclaration
        private Dictionary<Character,CombatAction.CombatAction> CharacterActions = 
            new Dictionary<Character, CombatAction.CombatAction>();

        public void ExecuteActions()
        {
            foreach (var character in CharacterActions.Keys)
                CharacterActions[character].Do(character);
            
            //remove defense, TODO rethink approach, maybe
            foreach (var character in CharacterActions.Keys)
            {
                character.IsDefending = false;
                character.IsActionAssigned = false;
            }

             
            CharacterActions.Clear(); ;
        }

        private enum ActionDeclaration
        { None, Attack, Skill, Item, DefendSelf, Escape}
        
        //Used for keeping control of the action declaration of a player character
        //E.g. Selecting an enemy after choosing the attack option
        private ActionDeclaration _partialActionType;
        private Character _currentCharacter;
        private CombatAction.CombatAction _currentAction;

        private void ResetPartialAction()
        {
            _partialActionType = ActionDeclaration.None;
            _currentCharacter = null;
            _currentAction = null;
        }

        public void BeginAttackDeclaration(testCharacterLoader attacker)
        {
            _currentCharacter = attacker.character;
            _partialActionType = ActionDeclaration.Attack;
            var temp = new CombatAction.Attack();
            _currentAction = temp;
        }

        public void AttributeTargetToAttack(testCharacterLoader target)
        {
            if (_partialActionType != ActionDeclaration.Attack ||
                _currentAction == null) return;

            var action = (_currentAction as CombatAction.Attack);
            action.Target = target.character;
            CharacterActions[_currentCharacter] = _currentAction;

            Debug.Log($"{_currentCharacter.CharacterName} will attack {target.character.CharacterName}");
            
            _currentCharacter.IsActionAssigned = true;
            ResetPartialAction();
            CloseActionMenus();
        }

        public void DefendDeclaration(testCharacterLoader defender)
        {
            CombatAction.Defend defend = new Defend();
            CharacterActions[defender.character] = defend;
            CloseActionMenus();
            defender.character.IsActionAssigned = true;
        }

        #endregion
}