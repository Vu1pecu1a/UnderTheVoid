using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class CharacterInfoWnd :WndBase 
{ 
   public override void Open()
    {
        base.Open();
        CharInfo info = new CharInfo("1¹ø", 10245, 22, 1);
        SettingCharInfo(info);
    }

    public override void Close()
    {
        base.Close();
    }

    void SettingCharInfo(CharInfo info)
    {

    }
}
