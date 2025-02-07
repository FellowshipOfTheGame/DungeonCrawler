using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillButtonLoader : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Skill buttonSkill;
    
    public void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSkill(Skill skill){
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (skill.IsUnityNull())
        {
            text.text = "----";
            return;
        }

        buttonSkill = skill;
        text.text = buttonSkill.skillName;
    }
}
