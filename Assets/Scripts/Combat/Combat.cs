using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatAction;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private static Combat _instance;
    public PlayerSave save;
    
    public List<Character> heroes;
    public List<Character> enemies;
    
    [SerializeField] private List<HeroPortrait> heroPortraits;
    [SerializeField] private GameObject enemyRoot;
    
    [SerializeField] private EnemyLayoutLoader enemyLayoutLoader;
    
    //TODO complete
    public void LoadEncounter(Encounter encounter)
    {
        print("Loading eNCONTUER");
        
        //Delete previous enemy portraits
        while (enemyRoot.transform.childCount > 0)
            DestroyImmediate(enemyRoot.transform.GetChild(0).gameObject);
        
        enemies.Clear();
        //Load Enemies List

        foreach (var encounterPosition in encounter.Formation)
        {
            var enemy = encounterPosition.Enemy;
            enemies.Add(enemy);
            print(enemy.GetClassName());
            
            var enemyObject = new GameObject(enemy.GetClassName());
            enemyObject.transform.SetParent(enemyRoot.transform);
            enemyObject.transform.position = enemyLayoutLoader.GetPosition(encounterPosition.Position);
            
            var enemyScript = enemyObject.AddComponent<Enemy>();
            enemyScript.Init(enemy);
        }
    } 
    
    
    public void Awake()
    {
        _instance = this;
        
        heroes = save.GetHeroes();
        
        for (int i = 0; i < heroes.Count; i++)
            heroPortraits[i].Setup(heroes[i]);

        StartCoroutine(TurnCoroutine());
    }
    
    private IEnumerator TurnCoroutine(){
        while (true) // Set ExitCondition
        {
            //Wait for all Heroes to take action
            yield return new WaitWhile
            (() => 
                heroes.Count(c => c.IsAlive) > characterActions.Count()
            );
           
            
            
            //ENEMIES ATTACK AT RANDOM
            EnemyCombatAI.Foo(this);
            
            ExecuteActions();
        }
    }

    public static Combat GetInstance()
    {
        if (_instance.IsUnityNull()) 
            throw new Exception("Uninstantiated combat reference.");
        return _instance;
    }

    public int Attack(Character attacker, Character target)
    {
        int damage = attacker.BaseDamage;
        int defense = target.BaseDefense;
        
        int damageDealt = Math.Max(attacker.BaseDamage - target.BaseDefense, 0);
        
        target.ReceiveDamage(damageDealt);
        return damageDealt;
    }
    
    #region ActionDeclaration
        private Dictionary<Character,CombatAction.CombatAction> characterActions = 
            new Dictionary<Character, CombatAction.CombatAction>();

        public void ExecuteActions()
        {
            foreach (var character in characterActions.Keys)
                characterActions[character].Do(character);
            
            //remove defense, TODO rethink approach, maybe
            foreach (var character in characterActions.Keys)
            {
                character.IsDefending = false;
                character.IsActionAssigned = false;
            }

             
            characterActions.Clear(); ;
        }

        private enum ActionDeclaration
        { None, Attack, Skill, Defend, Escape}
        
        //Used for keeping control of the action declaration of a player character
        //E.g. Selecting an enemy after choosing the attack option
        private ActionDeclaration partialActionType;
        private Character currentCharacter;
        private CombatAction.CombatAction currentAction;

        private void ResetPartialAction()
        {
            partialActionType = ActionDeclaration.None;
            currentCharacter = null;
            currentAction = null;
        }

        public void BeginAttackDeclaration(Character attacker)
        {
            partialActionType = ActionDeclaration.Attack;
            var temp = new Attack();
            currentCharacter = attacker;
            currentAction = temp;
        }

        public void AttributeTargetToAttack(Character target)
        {
            if (partialActionType != ActionDeclaration.Attack ||
                currentAction == null) return;

            var action = (currentAction as Attack);
            action.Target = target;
            characterActions[currentCharacter] = currentAction;

            Debug.Log($"{currentCharacter.characterName} will attack {target.characterName}");
            
            currentCharacter.IsActionAssigned = true;
            ResetPartialAction();
        }

        public void DefendDeclaration(Character defender)
        {
            Defend defend = new Defend();
            characterActions[defender] = defend;
            defender.IsActionAssigned = true;
            Debug.Log($"{defender.characterName} will defend!");
        }
        #endregion
}