using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenecManeger : MonoBehaviour
{
    public static ScenecManeger i;
    private void Start()
    {
        if (i != null)
        {
            Destroy(gameObject);
            return;
        }

        i = this;
        DontDestroyOnLoad(gameObject);
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
