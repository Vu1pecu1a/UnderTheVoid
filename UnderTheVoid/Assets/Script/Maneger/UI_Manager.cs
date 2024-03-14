using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_Char;
    [SerializeField] GameObject UI_inven;
    [SerializeField] GameObject Char;
    [SerializeField] GameObject ButtonUI;

    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//맵 생성 종료 이벤트(로딩 종료시)
    }

    public void UI_on()
    {
        if (isOn)
        {
            UI_Char.SetActive(true);
            UI_inven.SetActive(true);
            Char.SetActive(true);
            isOn= false;
        }else
        {
            UI_Char.SetActive(false);
            UI_inven.SetActive(false);
            Char.SetActive(false);
            isOn = true;
        }
    }
}
