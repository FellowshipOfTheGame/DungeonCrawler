using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillList : MonoBehaviour
{
    private List<SkillButtonLoader> skillDisplays;
    private Character character;

    public void Open(HeroPortrait characterLoader)
    {
        gameObject.SetActive(true);
        Start();//Gambiarra, TODO fazr o bagulho certo
        character = characterLoader.character;
        List<Skill> skills = character.GetSkills();
        int nDisplays = skillDisplays.Count;
        for (int i = 0; i < nDisplays; i++)
            if(i < skills.Count)
                skillDisplays[i].SetSkill(skills[i]);
            else
                skillDisplays[i].SetSkill(null);
    }

    public void Close()
    {
        character = null;
        this.gameObject.SetActive(false);
    }

    public void Start()
    {
        skillDisplays = GetComponentsInChildren<SkillButtonLoader>().ToList();
    }
}
