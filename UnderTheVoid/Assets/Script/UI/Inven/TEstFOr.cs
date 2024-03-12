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

    [SerializeField]
    List<GameObject> list = new List<GameObject>();
    private void Start()
    {
        w=a5.width; h=a5.height;
    }

    public void ChangeRoateBool(int a)
    {
       switch(a)
        {
            case 0:
                wh(w, h);
                break;
            case 1: //up
                hw(w, h);
                break;
            case 2: //down
                hwq(w, h);
                break;
            case 3:
                whq(w, h);
                break;
        }
    }

    void hw(int w,int h)
    {
        foreach (GameObject a in list)
        {
            Destroy(a);
        }
        list.Clear();

        //a5 = new bool[w, h];
        a6 = new bool[w * h];
        int c = 0;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                a6[c] = a5.IsPartOfShape(new Vector2Int(i, j));
                GameObject b = Instantiate(a);
                b.transform.position = new Vector3(i, 0, j);
                list.Add(b);

                if (a6[c] == true)
                    b.GetComponent<Renderer>().material.color = Color.red;
                c++;
            }
        }
    }

    void hwq(int w, int h)
    {
        foreach (GameObject a in list)
        {
            Destroy(a);
        }
        list.Clear();

        //a5 = new bool[w, h];
        a6 = new bool[w * h];
        int c = 0;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                a6[c] = a5.IsPartOfShape(new Vector2Int(i, j));
                GameObject b = Instantiate(a);
                b.transform.position = new Vector3(i, 0, j);
                list.Add(b);

                if (a6[c] == true)
                    b.GetComponent<Renderer>().material.color = Color.red;
                c++;
            }
        }

        Array.Reverse(a6);

        for(int i =0;i<a6.Length;i++)
        {
            if(a6[i]==true)
                list[i].GetComponent<Renderer>().material.color = Color.red;
            else
                list[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }


    void wh(int w,int h)
    {
        
        foreach (GameObject a in list)
        {
            Destroy(a);
        }
        list.Clear();

        //a5 = new bool[w, h];
        a6 = new bool[w * h];
        int c = 0;

        for(int i = 0; i < h;i++)
        {
            for(int j = 0; j<w;j++)
            {
                a6[c] = a5.IsPartOfShape90(new Vector2Int(j, i));
                GameObject b = Instantiate(a);
                b.transform.position = new Vector3(i,0, j);
                list.Add(b);

                if (a6[c] == true)
                    b.GetComponent<Renderer>().material.color = Color.red;
                c++;
            }
        }
        Debug.Log(a6);
    }
    void whq(int w, int h)
    {
        foreach (GameObject a in list)
        {
            Destroy(a);
        }
        list.Clear();

        //a5 = new bool[w, h];
        a6 = new bool[w * h];
        int c = 0;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                a6[c] = a5.IsPartOfShape90(new Vector2Int(j, i));
                GameObject b = Instantiate(a);
                b.transform.position = new Vector3(i, 0, j);
                list.Add(b);

                if (a6[c] == true)
                    b.GetComponent<Renderer>().material.color = Color.red;
                c++;
            }
        }
        Debug.Log(a6);
        Array.Reverse(a6);

        for (int i = 0; i < a6.Length; i++)
        {
            if (a6[i] == true)
                list[i].GetComponent<Renderer>().material.color = Color.red;
            else
                list[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
