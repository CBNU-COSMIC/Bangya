using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Camera _fpsCamera;
    [SerializeField] ParticleSystem _gunEffect;
    [SerializeField] float _damage = 30f;
    [SerializeField] float _range = 30f;
    [SerializeField] int _maxBullet = 10;
    [SerializeField] int _haveBullet = 100  
    int Bullet = 10; //�ʱⰪ
    bool IsAiming = false;

    void Start()
    {
        Bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Bullet <= 0) //�Ѿ� ����
            {
                Debug.Log("No Bullet");
                return;
            }
            Shoot(); //�߻�
        }

        if (Input.GetButtonDown("Fire2")) Aim();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_haveBullet <= 0) //������ �ִ� �� ����
            {
                Debug.Log("No Bullet");
                return;
            }
            if (Bullet == _maxBullet) //�̹� �� ��
            {
                Debug.Log("It's full");
                return;
            }
            Debug.Log("Loading");
            Load();
        }
    }

    void Shoot()
    {
        _gunEffect.Play(); //����Ʈ (���߿� ���� �� ã���Կ�)
        _player.transform.Translate(0, 0, -0.1f); //�׳� �ݵ� ���� ��
        _player.transform.Rotate(0.1f, 0.1f, 0); //�̰͵� �׳� ���� ��
        Fire(); //��¥ �߻�
        Bullet--;
    }
    void Fire()
    {
        RaycastHit Hit;
        if(Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out Hit, _range))
        {
            EnemyHealth target = Hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            //HitEffect(); // ���� �� ��¦
            target.TakeDamage(_damage);
        }
    }

    void Aim()
    {
        if (!IsAiming)
        {
            IsAiming = true;
            Debug.Log("Aiming");
        }
        else
        {
            Debug.Log("Stop Aiming");
            IsAiming = false;
        }
        //�� �� �� �� �� ?
    }

    void Load()
    {
        //������?
        int AddBullet = _maxBullet - Bullet;
        if(AddBullet > _haveBullet)
        {
            Bullet += _haveBullet;
            _haveBullet = 0;
            return;
        }
        _haveBullet -= AddBullet;
        Bullet += AddBullet;
    }
}
