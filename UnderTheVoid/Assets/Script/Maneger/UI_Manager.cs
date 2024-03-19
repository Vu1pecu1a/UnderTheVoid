using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_Char;
    [SerializeField] GameObject UI_inven;
    [SerializeField] GameObject Char;
    [SerializeField] GameObject ButtonUI;
    [SerializeField] GameObject GameOver_UI;

    [SerializeField] Transform overButton;

    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//맵 생성 종료 이벤트(로딩 종료시)
        overButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
    }


    public void UIManager_GameOver()
    {
        GameOver_UI.SetActive(true);
    }

    public void UI_on()
    {
        if (isOn)
        {
            UI_Char.SetActive(true);
            UI_inven.transform.localPosition = new Vector2(1560, -475);
            Char.SetActive(true);
            isOn= false;
        }else
        {
            UI_Char.SetActive(false);
            UI_inven.transform.localPosition = new Vector2(3560, -475);
            Char.SetActive(false);
            isOn = true;
        }
    }
}