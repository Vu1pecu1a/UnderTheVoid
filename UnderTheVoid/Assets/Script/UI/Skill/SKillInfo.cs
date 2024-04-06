using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SKillInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Name, _Info, _Multiplier;
    [SerializeField] Image _SKillSprite;

    public void SkillSet(Skill skill)
    {
        _Name.text = skill._Name;
        _Info.text = skill._Info;
        _Multiplier.text = skill._Multiplier.ToString();
        _SKillSprite.sprite = skill.SkillImage;
    }
}
