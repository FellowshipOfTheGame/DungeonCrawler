using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


public class testCharacterLoader : MonoBehaviour
{
    [HideInInspector]public Character character;
    
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterClassName;
    public TextMeshProUGUI characterLevel;
    
    public Slider _sliderHealth;
    public Slider _sliderMana;

    public Image portraitImage;
    public GameObject ActionMenu;
    
    public GameObject shieldIcon;
    public GameObject actionIndicator;
    public void Setup(Character character)
    {
        this.character = character;
        if(!character.initialized) character.Initialize();
        LoadCharacter();
        character.OnChange += LoadCharacter;
    }

    void OnDisable()
    {
        character.OnChange -= LoadCharacter;    
    }

    void LoadCharacter()
    {
        characterName.text = character.CharacterName;
        characterClassName.text = character.GetClassName();
        characterLevel.text = character.Level.ToString();
        
        _sliderHealth.value = (float)character.Health/character.MaxHealth;
        _sliderMana.value = (float)character.Mana/character.MaxMana;
        
        portraitImage.sprite = character.GetImage();
        shieldIcon.SetActive(character.IsDefending);
        actionIndicator.SetActive(character.IsActionAssigned);
    }
}
