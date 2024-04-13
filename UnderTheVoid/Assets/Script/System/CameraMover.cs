using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    float movespeed =1;
    bool Stop;
    static float _timeScale = 1;
    public void TimeScaleUp()
    {
        if (Time.timeScale < 4)
            _timeScale += 1f;
        SetTimeScale();
        Managers.instance._UI.SetTime();
    }
    public void TimeScaleDown()
    {
        if (Time.timeScale > 0)
            _timeScale -= 1f;
        SetTimeScale();
        Managers.instance._UI.SetTime();
    }

    public static void SetTimeScale()
    {
        Time.timeScale = _timeScale;
    }

    public void Puase()
    {
        if (Stop)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(Vector3.forward*movespeed);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(Vector3.back * movespeed);
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(Vector3.right * movespeed);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(Vector3.left * movespeed);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.transform.Rotate(0,90,0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.transform.Rotate(0, -90, 0);
        }

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0 && !Managers.instance._UI.isOn)
        {
            if (Camera.main.orthographicSize > 2)
                Camera.main.orthographicSize--;
        }
        else if (wheelInput < 0 && !Managers.instance._UI.isOn)
        {
            Camera.main.orthographicSize++;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Stop==true)
            Stop = false;
            else
                Stop= true;
            Puase();
        }
    }
}
