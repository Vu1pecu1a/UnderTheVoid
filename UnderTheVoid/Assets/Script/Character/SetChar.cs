using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetChar : MonoBehaviour
{
    public Image a;
    public TextMeshProUGUI text;

    public void SetIMage(int a)
    {
       text.text = ScenecManeger.i.excleSystem.strings[a];
    }
}
