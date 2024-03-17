using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsingleton<T> : MonoBehaviour where T : Tsingleton<T>
{
    static volatile T _uniqueInstance = null;
    static volatile GameObject _uniqueObject = null;

    public static T _instance
    {
        get 
        {
            if (_uniqueInstance == null)
            {
                lock(typeof(T))
                if(_uniqueInstance ==null && _uniqueObject == null)
                {
                    _uniqueObject = new GameObject(typeof(T).Name, typeof(T));
                    _uniqueInstance = _uniqueObject.GetComponent<T>();

                    _uniqueInstance.Init();
                }
            }
            return _uniqueInstance;
        }
    }

    protected Tsingleton()//생성자
    {

    }//상속 전용 클래스로 만들겠다는 내용

    protected virtual void Init()
    {
        DontDestroyOnLoad(gameObject);
    }
}
