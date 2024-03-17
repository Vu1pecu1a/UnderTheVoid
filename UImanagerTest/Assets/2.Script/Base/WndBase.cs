using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WndBase : MonoBehaviour
{
    /// <summary>
    /// 윈도우 오픈
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 윈도우 클로즈
    /// </summary>
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
