using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TEstFOr : MonoBehaviour
{
    [SerializeField]
    GameObject a;
    public InvenShape a5;
    public bool[] a6;
    int w = 0;
    int h = 0;

    List<GameObject> list = new List<GameObject>();
    private void Start()
    {
        w=a5.width; h=a5.height;
        wh(w, h);
    }
    void wh(int w,int h)
    {
        //a5 = new bool[w, h];
        a6 = new bool[w * h];
        int c = 0;

        for(int i = 0; i < w;i++)
        {
            for(int j = 0; j<h;j++)
            {
                a6[c] = a5.IsPartOfShape(new Vector2Int(i, j));
                GameObject b = Instantiate(a);
                b.transform.position = new Vector3(j,0, i);
                list.Add(b);

                if (a6[c] == true)
                    b.GetComponent<Renderer>().material.color = Color.red;
                c++;
            }
        }

        //for(int y=w;y>=0;y--)
        //{
        //    for (int x = h; x >= 0; x--)
        //    {
        //        c--;
        //        a6[c] = a5.IsPartOfShape(new Vector2Int(y,x));
        //        Debug.Log(a5.IsPartOfShape(new Vector2Int(y, x)));
        //        GameObject b = Instantiate(a);
        //        b.transform.position = new Vector3(x,0, y);
        //        if(a6[c]==true)
        //            b.GetComponent<Renderer>().material.color = Color.red;
        //    }
        //}
        Debug.Log(a6);
    }

}
