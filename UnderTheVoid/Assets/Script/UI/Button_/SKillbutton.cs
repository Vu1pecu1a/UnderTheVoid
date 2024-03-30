using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SKillbutton : MonoBehaviour
{
    public Skill skill;
    public Toggle togglebu;
    public PlayerBase playerbase;
    public int Count;
   public void SetSkill()
    {
        Count = togglebu.transform.GetSiblingIndex();
        togglebu.transform.GetChild(0).GetComponent<Image>().sprite = skill.SkillImage;
        if (Count < 2 && skill is ActiveSkill)
            playerbase.Skills[Count] = skill;
        else
            playerbase.SetPassiveSkill(Count - 2, skill);
          //  playerbase.PasiveSkills[Count-2] = skill;
    }
}
