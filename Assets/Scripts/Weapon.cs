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
    int Bullet = 10; //초기값
    bool IsAiming = false;

    void Start()
    {
        Bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Bullet <= 0) //총알 없음
            {
                Debug.Log("No Bullet");
                return;
            }
            Shoot(); //발사
        }

        if (Input.GetButtonDown("Fire2")) Aim();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_haveBullet <= 0) //가지고 있는 거 없음
            {
                Debug.Log("No Bullet");
                return;
            }
            if (Bullet == _maxBullet) //이미 꽉 참
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
        _gunEffect.Play(); //이펙트 (나중에 좋은 거 찾을게요)
        _player.transform.Translate(0, 0, -0.1f); //그냥 반동 실험 중
        _player.transform.Rotate(0.1f, 0.1f, 0); //이것도 그냥 실험 중
        Fire(); //진짜 발사
        Bullet--;
    }
    void Fire()
    {
        RaycastHit Hit;
        if(Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out Hit, _range))
        {
            EnemyHealth target = Hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            //HitEffect(); // 맞은 곳 반짝
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
        //뭘 해 야 할 까 ?
    }

    void Load()
    {
        //딜레이?
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
