using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers instance;

    [SerializeField] KeyInputManager keyInputManager;

    public delegate void Rkey();
    public event Rkey RkeyInput;

    // Start is called before the first frame update
    void Awake()
    {   
        instance= this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            RkeyInput();
        }
    }
}
