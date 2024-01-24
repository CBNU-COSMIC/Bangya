using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _speed = 5;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }

    public enum PlayerState
    {
        Moving,
        Idle,
    }
    PlayerState _state = PlayerState.Idle;

    void UpdateMoving()
    {
        // 이동 및 회전
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }


        if (Input.anyKey == false)
        {
            _state = PlayerState.Idle;
        }

        // 애니메이션 재생
        Animator anim = GetComponent<Animator>();
        anim.SetBool("IsRun", true);
    }
    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("IsRun", false);
    }

    void OnKeyboard(Define.KeyEvent evt)
    {
       _state = PlayerState.Moving;
    }

    void Update() // 상태에 따른 애니메이션 재생
    {
        switch(_state)
        {
            case PlayerState.Moving:
                UpdateMoving(); 
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }
}