using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Character character;

    public void Init(Character enemyCharacter)
    {
        character = enemyCharacter;
        SetupEnemy();
    }

    private void SetupEnemy()
    {
        Combat combat = Combat.GetInstance();
        if(combat.IsUnityNull()) throw new NullReferenceException();

        //Combat Action Setup
        var button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(
            () => combat.AttributeTargetToAttack(character)
        );
        
        //Image Setup
        var image = gameObject.AddComponent<Image>();
        image.sprite = character.characterClass.Portraits[0];

        character.Initialize();
    }
}
