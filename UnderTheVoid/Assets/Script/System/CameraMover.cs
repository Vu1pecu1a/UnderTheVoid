using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    float movespeed =1;

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
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            if (Camera.main.orthographicSize > 2)
                Camera.main.orthographicSize--;
        }
        else if (wheelInput < 0)
        {

            Camera.main.orthographicSize++;
        }
            
    }
}
