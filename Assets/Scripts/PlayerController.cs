using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField] float _walkSpeed;
    [SerializeField] float _runSpeed;

    public float applySpeed;   // 현재 속도

    [SerializeField] float _jumpPower;    // 점프의 크기


    // 상태 변수
    bool isWalk = false;
    bool isRun = false; // 달리기 상태 체크 변수
    bool isGround = true; // 땅위에 있는지 체크 변수


    // 움직임 체크 변수
    Vector3 lastPos;    // 이전 프레임의 위치정보


    // 땅과의 충돌을 체크하기 위해, 땅 착지 여부
    CapsuleCollider capsuleCollider;


    // 카메라 민감도
    [SerializeField] float loockSensitivity;


    // 카메라 한계
    [SerializeField] float cameraRotationLimit;  // 카메라 회전 제한, 제한 안 하면 무한 회전함.
    float currentCameraRotationX = 0f; // 정면


    // 컴포넌트
    [SerializeField] Camera _camera;
    [SerializeField] Animator _anim;
    Rigidbody myRigid;


    void Start()
    {
        // theCamera = FindObjectOfType<Camera>();     // 계층구조 내의 카메라 객체를 가져옴
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();

        // 초기화
        applySpeed = _walkSpeed;     // 걷는 것이 초기 상태
    }

    void Update()
    {
        Jump();
        IsGround();
        Move();
        Run();
        CameraRotation();
        CharacterRotation();
    }

    void FixedUpdate()
    {
        MoveCheck();
    }

    // 지면 유저가 키 인풋을 발생시켰는지 체크
    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxRayDistance = 0.1f;
        
        Debug.DrawRay(origin, direction * maxRayDistance, Color.red);
        
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxRayDistance))
        {
            Debug.Log("땅에 닿았다!");
            _anim.SetBool("IsJump", false);
        }
    }

    void Idle()
    {
        _anim.SetBool("IsWalk", false);
    }

    void Walk()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        // 유니티에선 z축이 앞뒤, 방향 선언
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // 합이 1이 나오도록 정규화, 계산의 용이성을 위해
        // 방향 * 속도
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        // 자연스러운 이동을 위해 delta time(0.016)을 곱함. 아니면 순간이동
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        _anim.SetBool("IsWalk", true);
    }
    void Run() //유저가 키 인풋을 발생시켰는지 체크
    {
        // GetKey = 키를 계속 누르고 있는 상태
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            applySpeed = _runSpeed;
        }
        // GetkeyUp 키를 땐 상태
        if (Input.GetKeyUp(KeyCode.LeftShift)) // 달리기 -> 걷기
        {
            isRun = false;
            applySpeed = _walkSpeed;
        }
    }
    void Jump() //점프가 가능한 상태인지 체크
    {
        // GetKeyDown = 한번만
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            myRigid.velocity = transform.up * _jumpPower;
            _anim.SetBool("IsJump", true);
        }
    }

    void Move() //플레이어의 움직임을 조절하는 함수
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
        }
        else
        {
            Idle();
        }
    }

    void MoveCheck() // 이동이 있었는지 체크
    {
        if (!isRun && isGround)
        {
            // 경사로에 대한 여유를 줌
            if (Vector3.Distance(lastPos, transform.position) >= 0.02f) isWalk = true;
            else isWalk = false;

            lastPos = transform.position;
        }
    }

    void CharacterRotation() // 플레이어의 좌우 회전을 담당하는 함수
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * loockSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    void CameraRotation() //카메라 상하 회전을 담당하는 함수
    {
        // 마우스는 2차원, X랑 Y만 있음
        float _xRotiation = Input.GetAxis("Mouse Y");   // 마우스 위 아래 == 유니티 x축
        float _cameraRotationX = _xRotiation * loockSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // 유니티는 회전을 쿼터니언으로 내부적으로 처리한다.
        _camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); // Rotation x, y, z
    }
}