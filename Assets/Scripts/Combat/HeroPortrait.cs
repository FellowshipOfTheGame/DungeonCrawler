using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


public class HeroPortrait : MonoBehaviour
{
    [HideInInspector]public Character character;
    
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterClassName;
    public TextMeshProUGUI characterLevel;
    
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    public Image portraitImage;
    
    public GameObject shieldIcon;
    public GameObject actionIndicator;

    public void Setup(Character hero)
    {
        character = hero;
        if(!character.initialized) character.Initialize();
        LoadCharacter();
        character.OnChange += LoadCharacter;
    }

    void OnDisable() =>
        character.OnChange -= LoadCharacter;   

    void LoadCharacter()
    {
        portraitImage.sprite = character.GetImage();
        
        healthText.text = $"{character.health}/{character.maxHealth}";
        manaText.text = $"{character.mana}/{character.maxMana}";
        
        shieldIcon.SetActive(character.IsDefending);
        actionIndicator.SetActive(character.IsActionAssigned);
    }
}
