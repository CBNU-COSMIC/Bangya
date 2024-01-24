using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class InputManager
{
    public Action<Define.KeyEvent> KeyAction = null;
    
    public void OnUpdate()
    {
        if (Input.anyKey == false) return;

        if (Input.GetKey(KeyCode.W))
        {
            KeyAction.Invoke(Define.KeyEvent.W);
        }
        if (Input.GetKey(KeyCode.S))
        {
            KeyAction.Invoke(Define.KeyEvent.S);
        }
        if (Input.GetKey(KeyCode.A))
        {
            KeyAction.Invoke(Define.KeyEvent.A);
        }
        if (Input.GetKey(KeyCode.D))
        {
            KeyAction.Invoke(Define.KeyEvent.D);
        }
    }
}
