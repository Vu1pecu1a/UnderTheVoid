using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager 
{
    // Delegate 
    public Action keyaction = null;

    
    // InputMangers will detect inputs in OnUdate()
    public void OnUpdate()
    {
        // �Է� ���� Ű�� �ƹ��͵� ���ٸ� ����
        if (Input.anyKey == false) return;

        // � Ű�� ���Դٸ�, keyaction���� �̺�Ʈ�� �߻������� ����. 
        if (keyaction != null)
        {
            keyaction.Invoke();
        }
    }
}
