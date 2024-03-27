using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill 
{
    public abstract float CoolTIme { get; }
    public abstract Sprite SkillImage { get;}
    public abstract void SkillOn(MonsterBase pb);//��ų ��� ȿ��

}

public class RapidFire : Skill
{
    [SerializeField,Tooltip("��ų ��Ÿ��")] float _coolTime;
    [SerializeField, Tooltip("��ų �̹���")] Sprite _sprite;

    public override float CoolTIme { get => _coolTime; }
    public override Sprite SkillImage { get => _sprite; }

    public RapidFire(float coolTime, Sprite sprite)
    {
        _coolTime = coolTime;
        _sprite = sprite;
    }

    public override void SkillOn(MonsterBase pb)
    {
        pb.AddBuff(pb, pb, BuffType.AdSpeedBuff, 60, 60);
    }
}