using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM <T> : MonoBehaviour
{
    T owner; // 상태 소유자
    InterfaceFsmState<T> currentState = null; //현재 상태
    InterfaceFsmState<T> previousState = null; // 이전 상태

    public InterfaceFsmState<T> CurrentState { get { return currentState; } }
    public InterfaceFsmState<T> PreviousState { get { return previousState; } }
    //캡슐화

    protected void InitState(T owner,InterfaceFsmState<T> state)
    {
        this.owner = owner;
        ChageState(state);
    }

    protected void FsmUpdate()//상태 진행용 함수
    { if (currentState != null) currentState.Execute(owner); }

    public void ChageState(InterfaceFsmState<T> state)//상태 교체용 함수
    {
        previousState = currentState;//이전 상태 = 현재 상태

        if(previousState != null)
        {
            previousState.Exit(owner);//소유주의 이전 상태를 종료시킴
        }

        currentState = state;
        if(currentState != null)//방어코드
        {
            currentState.Enter(owner);
        }
    }

   public void RevertState()//이전상태로 되돌아가게 하는 함수
    {
        if(previousState != null)
            ChageState(previousState);
    }

    public override string ToString()
    {
        return currentState.ToString();
    }
}
public class FSMSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    //----------------------------------------
    private static T _instance;
    private static object _lock = new object();
    //----------------------------------------
    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("--- FSMSingleton error ---");
                        return _instance;

                    }//	if ( FindObjectsOfType(typeof(T)).Length > 1 )

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        singleton.hideFlags = HideFlags.HideAndDontSave;

                    }//	if (_instance == null)
                    else
                        Debug.LogError("--- FSMSingleton already exists ---");

                }//	if (_instance == null)

                return _instance;

            }//	lock(_lock)

        }//	get

    }//	public static T Instance
     //----------------------------------------

}