using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetChar : MonoBehaviour
{
    public Image a;
    public TextMeshProUGUI text;

    public int _index { get; private set; }

    public void SetIMage(int a)
    {
       text.text = ScenecManeger.i.excleSystem.strings[a];
        _index = a;
    }

    public void SetImage(bool PM)
    {
        SetIMage(isIndex(PM));
    }

    public int isIndex(bool PM)
    {
        if(PM)
        {
            if(_index+1 >= ScenecManeger.i.excleSystem.strings.Count)
            {
                return 0;
            }
            else
            {
                return _index + 1;
            }
        }
        else
        {
            if (_index -1 < 0)
            {
                return ScenecManeger.i.excleSystem.strings.Count -1;
            }
            else
            {
                return _index - 1;
            }
        }
    }
}
