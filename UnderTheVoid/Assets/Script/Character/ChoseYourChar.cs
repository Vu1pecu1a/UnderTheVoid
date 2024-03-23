using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoseYourChar : MonoBehaviour
{
    public GameObject[] EmpltChar; 
    private void OnEnable()
    {
        int b = 0;
        foreach(GameObject a in EmpltChar)
        {
            if (a.GetComponent<SetChar>().sprite == null)
            {
                a.GetComponent<SetChar>().sprite = a.transform.GetChild(0).GetComponent<Image>();
            }
            a.GetComponent<SetChar>().SetIMage(b);
        }
    }
}
