using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenecManeger : MonoBehaviour
{
    public ExcelParsingSystem excleSystem { get; set; }

    public static ScenecManeger i;

    [SerializeField]
    GameObject[] Char;

    public List<PlayerBase> CharList { get; private set; }

    public List<GameObject> ChoseCharicterList;

    private void Awake()
    {
        
        if (i != null)
        {
            Destroy(gameObject);
            return;
        }

        i = this;
        excleSystem = GetComponent<ExcelParsingSystem>();
        DontDestroyOnLoad(gameObject);
        SetUP();
    }

    void SetUP()
    {
        for(int i = 0; i < Char.Length;i++)
        {
            ChoseCharicterList.Add(Char[i]);
        }
    }//남은 캐릭터 리스트
    

    public void SetChar(int a)
    {
        CharList.Add(Char[a].GetComponent<PlayerBase>());
    }

    public void GoScene(int Alfa)
    {
        SceneManager.LoadScene(Alfa);
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
