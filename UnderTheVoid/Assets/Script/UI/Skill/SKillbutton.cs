using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SKillbutton : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    public Skill skill;
    public Toggle togglebu;
    public PlayerBase playerbase;
    public int Count;

    public static GameObject SkillInfo;

    public void Start()
    {
        SkillInfo = Managers.instance._UI.SkilltoolTip();
    }

    public void SetSkill()
    {
        Count = togglebu.transform.GetSiblingIndex();
        togglebu.transform.GetChild(0).GetComponent<Image>().sprite = skill.SkillImage;

        if (Count < 2 && skill is ActiveSkill)
            playerbase.Skills[Count] = (Skill)skill.Clone();
        else
            playerbase.SetPassiveSkill(Count - 2, (Skill)skill.Clone());
        //  playerbase.PasiveSkills[Count-2] = skill;

        if(Count==0)//스킬 사용 버튼이미지 변경
        Managers.instance._UI.Skill_button[D_calcuate.i.PlayerList.FindIndex(s => s == playerbase)].transform.GetChild(1).GetComponent<Image>().sprite = skill.SkillImage;

        togglebu.SetIsOnWithoutNotify(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("마우스 오버");
        SkillInfo.SetActive(true);
        SkillInfo.GetComponent<RectTransform>().position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, Screen.width), Mathf.Clamp(this.transform.position.y, 0, Screen.height));
        SkillInfo.GetComponent<SKillInfo>().SkillSet(skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("마우스 탈출");
        SkillInfo.SetActive(false);
    }
}
