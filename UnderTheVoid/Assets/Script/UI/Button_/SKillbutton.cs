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
        playerbase.Skills[Count] = skill;
    }
}
