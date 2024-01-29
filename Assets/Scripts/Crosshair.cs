using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    RawImage _crosshair;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
    }
    void Update()
    {
        //Vector3 mousePosition = Input.mousePosition;
        //_crosshair.transform.position = mousePosition;
    }
}
