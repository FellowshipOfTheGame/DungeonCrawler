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

    public (float, int) NewAttack(Character attacker, Character target, float physOrMagicBaseDamage, int damageType)
    {
        // TODO: considerar possibilidade de critico
        // TODO: quando os enums forem criados, trocar if por switch

        // NOVA FUNCAO DE ATAQUE, NAO USAR COM O NOME NewAttack(), SUBSTITUIR Attack() com o codigo nesta funcao

        // damageType deve substituir por um enum, no momento:
        // 0 = fisico, 1 = magico

        /*
         * Funcao que aplica o dano de um ataque ao alvo, levando em conta o tipo de dano
         * 
         * attacker (Character): Personagem que esta atacando
         * target (Character): Personagem que esta sendo atacado
         * physOrMagicBaseDamage (float): Dano base do ataque, que pode ser calculado por 
         *          AttributeCalculation.AttackCalculation() ou AttributeCalculation.MagicCalculation()
         * damageType (int): tipo do dano, substitituir por um enum no futuro
         * 
         * return (int, int): [0] = dano causado (negativo se absorvido), [1] = alvo do dano (0 = personagem atacando, 1 = personagem atacado)
         */

        // Trocar por enum[] e pegar de Character.typeCharacteristics
        // enum[damageType] retorna como o personagem reage a um tipo de dano
        // por enquanto 0 = normal, 1 = resiste, 2 = repel, 3 = absorb
        int[] attackerTypeCharacteristics = { 0, 0 };
        int[] targetTypeCharacteristics = { 0, 0 };

        int attackerDefense = attacker.BaseDefense;
        int targetDefense = target.BaseDefense;

        float damage;
        int damageInteger;

        if (targetTypeCharacteristics[damageType] == 2) // alvo repele o dano
        {
            damage = AttributeCalculation.DamageCalculation(
                physOrMagicBaseDamage,
                attackerDefense,
                0.1f
            );

            if (attackerTypeCharacteristics[damageType] == 2) // atacante repele o dano
            {
                // Ambos repelem, dano = 0
                Debug.Log("Repelencia mutua");
                return (0, 0);
            }
            else if (attackerTypeCharacteristics[damageType] == 3) // atacante absorve o dano
            {
                Debug.Log("Alvo repeliu e atacante absorveu");
                // funcao de curar deve ser chamada aqui
                return (-damage, 0);
            }

            if(attackerTypeCharacteristics[damageType] == 1) // atacante resiste
            {
                Debug.Log("Alvo repeliu e atacante resiste");
                damage /= 2;
            }

            damageInteger = (int)MathF.Ceiling(damage);

            // atacante normal ou resiste
            Debug.Log("Alvo repeliu, atacante normal ou resiste");
            attacker.ReceiveDamage(damageInteger);
            return (damage, 0);
        }
        else if (targetTypeCharacteristics[damageType] == 3) // alvo absorve o dano
        {
            damage = -1 * AttributeCalculation.DamageCalculation(
                physOrMagicBaseDamage,
                targetDefense,
                0.1f
            );

            damageInteger = (int)MathF.Ceiling(damage);

            Debug.Log("Alvo absorveu o dano");
            // funcao de curar deve ser chamada aqui
            return (-damageInteger, 1);
        }

        // Alvo normal ou resiste
        damage = AttributeCalculation.DamageCalculation(
            physOrMagicBaseDamage,
            targetDefense,
            0.1f
        );

        if (targetTypeCharacteristics[damageType] == 1) // alvo resiste
        {
            Debug.Log("Alvo resiste ao dano");
            damage /= 2;
        }

        Debug.Log("Alvo normal ou resiste, dano aplicado");

        damageInteger = (int)MathF.Ceiling(damage);

        target.ReceiveDamage(damageInteger);

        return (damage, 1);
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