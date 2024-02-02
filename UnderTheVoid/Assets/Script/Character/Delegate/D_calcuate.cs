using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class D_calcuate : MonoBehaviour
{
    public static D_calcuate i;
    delegate void CallintD(int d);
    public delegate int PlayerHit(MonsterBase A,int B);
    public delegate void PlayerDie();

    event CallintD d;
    public static event PlayerHit playerHit;
    public static event PlayerDie playerDie;


    public static EventHandler hit;
    delegate int intop(int x,int y);
    
    intop op;

    void ReturnD(MonsterBase target,int D)
    {
        target.HP -= D;
    }

    // Start is called before the first frame update
    void Start()
    {
        i = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerDie();
        }
    }
}
