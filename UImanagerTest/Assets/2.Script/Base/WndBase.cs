using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WndBase : MonoBehaviour
{
    /// <summary>
    /// ������ ����
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ������ Ŭ����
    /// </summary>
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
