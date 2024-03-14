using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Move : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Managers.instance.Tab += UIMove;
    }

    private void OnDisable()
    {
        
    }
    
    void UIMove()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
