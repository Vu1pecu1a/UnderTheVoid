using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseYourChar : MonoBehaviour
{
    public GameObject[] EmpltChar; 
    private void OnEnable()
    {
        int b = 0;
        foreach(GameObject a in EmpltChar)
        {
            if (a.GetComponent<SetChar>().a==null)
            a.GetComponent<SetChar>().SetIMage(b);
        }
    }
}
