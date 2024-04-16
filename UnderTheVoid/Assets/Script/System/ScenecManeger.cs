using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenecManeger : Tsingleton<ScenecManeger>
{
    public ExcelParsingSystem excleSystem { get; set; }

    public static ScenecManeger i; 

    [SerializeField]
    GameObject[] Char;

    public List<PlayerBase> CharList { get; private set; }

    public List<GameObject> ChoseCharicterList;

    private void Awake()
    {
        i = this;
        excleSystem = GetComponent<ExcelParsingSystem>();
        //if (i != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //
        //excleSystem = GetComponent<ExcelParsingSystem>();
        //DontDestroyOnLoad(gameObject);
    }


    public void SetUP(ChoseYourChar sh)
    {
        FadeInout fade = this.GetComponent<FadeInout>();
        fade.Fade();
        List<int> index = new List<int>();
        foreach(GameObject a in sh.EmpltChar)
        {
            index.Add(a.GetComponent<SetChar>()._index);
        }
        for (int i = 0; i < index.Count;i++)
        {
            ChoseCharicterList.Add(Char[index[i]]);
        }
        GoScene(1);
    }//남은 캐릭터 리스트
    
    public ItemDefinition PlayerstartItem(int i)
    {
        return Char[i].GetComponent<PlayerBase>()._definitions[0];//첫번째 아이템만 리턴
    }
    public int PlayerstartSkill(int i)
    {
        int s = Char[i].GetComponent<PlayerBase>()._StartSkill;
        return s;//스킬 번호 리턴
    }

    public void SetChar(int a)
    {
        CharList.Add(Char[a].GetComponent<PlayerBase>());
    }

    public void GoScene(int Alfa)
    {
        SceneManager.LoadSceneAsync(Alfa);
        Time.timeScale = 1;
    }

    public void GameExit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
