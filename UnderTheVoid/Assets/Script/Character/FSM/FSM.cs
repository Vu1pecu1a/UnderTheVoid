using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM <T> : MonoBehaviour
{
    T owner; // ���� ������
    InterfaceFsmState<T> currentState = null; //���� ����
    InterfaceFsmState<T> previousState = null; // ���� ����

    public InterfaceFsmState<T> CurrentState { get { return currentState; } }
    public InterfaceFsmState<T> PreviousState { get { return previousState; } }
    //ĸ��ȭ

    protected void InitState(T owner,InterfaceFsmState<T> state)
    {
        this.owner = owner;
        ChageState(state);
    }

    protected void FsmUpdate()//���� ����� �Լ�
    { if (currentState != null) currentState.Execute(owner); }

    public void ChageState(InterfaceFsmState<T> state)//���� ��ü�� �Լ�
    {
        previousState = currentState;//���� ���� = ���� ����

        if(previousState != null)
        {
            previousState.Exit(owner);//�������� ���� ���¸� �����Ŵ
        }

        currentState = state;
        if(currentState != null)//����ڵ�
        {
            currentState.Enter(owner);
        }
    }

   public void RevertState()//�������·� �ǵ��ư��� �ϴ� �Լ�
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