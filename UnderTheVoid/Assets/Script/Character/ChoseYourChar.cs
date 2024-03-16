using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseYourChar : MonoBehaviour
{
    [SerializeField]GameObject[] EmpltChar; 
    private void OnEnable()
    {
        int b = 0;
        foreach(GameObject a in EmpltChar)
        {
            a.GetComponent<SetChar>().SetIMage(b);
                b++;
        }
    }

}
