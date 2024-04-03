using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers instance;


    [SerializeField] KeyInputManager KeyInputManager;
    [SerializeField] D_calcuate D_calcuate;
    [SerializeField] ChaterManager ChaterManager;
    [SerializeField] UI_Manager UIManager;

    #region[������]
     public KeyInputManager _keyInputManager { get => KeyInputManager; set => KeyInputManager = value; }
     public D_calcuate _D { get => D_calcuate; set => D_calcuate = value; }
     public ChaterManager _C { get => ChaterManager; set => ChaterManager = value; }
    public UI_Manager _UI { get => UIManager; set => UIManager = value; }
    #endregion[������]


    public delegate void Rkey();
    public delegate void Tap();
    public event Rkey RkeyInput;
    public event Tap Tab;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
       // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (D_calcuate == null) D_calcuate = D_calcuate.i;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) RkeyInput();
        if (Input.GetKeyUp(KeyCode.Tab)) Tab();
    }

    public void InputTAB()
    {
        Tab();
    }
}
