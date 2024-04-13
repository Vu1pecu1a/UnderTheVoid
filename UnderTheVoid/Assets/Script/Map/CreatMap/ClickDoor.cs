using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDoor : MonoBehaviour
{
    [SerializeField]
    int i;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
           Debug.Log("¿¡·¯");
            return;
        }
        Managers.instance._fd.Fade();
        StartCoroutine(PMTM());
    }

    IEnumerator PMTM()
    {
        yield return new WaitForSeconds(0.5f);
        MapGenerator.i.PlayerMoveToMap(i);
    }
}
