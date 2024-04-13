using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField] GameObject Inven;
    private void OnMouseUp()
    {
        if (Inven.activeSelf == false)
        {
            if(Managers.instance._UI.isOn==false)
            Managers.instance.InputTAB();

            Managers.instance.InputEscape();
        }
        Inven.SetActive(true);
        Inven.transform.SetParent(Managers.instance._UI.RewardPanel);
        
        Managers.instance.Tab += EscInven;
    }

    public void EscInven()
    {
        Inven.SetActive(false);
        Inven.transform.SetParent(transform);
        Managers.instance.Tab -= EscInven;
    }
}
