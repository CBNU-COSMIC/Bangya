using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject _hitEffect;
    [SerializeField] GameObject _player;
    [SerializeField] Camera _fpsCamera;
    [SerializeField] ParticleSystem _gunEffect;
    [SerializeField] float _damage = 30f; //총 위력
    [SerializeField] float _range = 30f;  //총 범위
    [SerializeField] int _maxBullet = 10; //최대 탄환 수
    [SerializeField] int _haveBullet = 100; //가지고 있는 탄환 수
    int Bullet = 10; //초기값
    bool IsAiming = false;
    bool IsReloading = false;

    void Start()
    {
        Bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //발사
        {
            if (Bullet <= 0) //총알 없음
            {
                Debug.Log("No Bullet");
                return;
            }
            if (!IsReloading) Shoot(); //발사
            else return;
        }

        else if (Input.GetButtonDown("Fire2")) Aim();  //조준

        else if (Input.GetKeyDown(KeyCode.R)) //장전
            {
                if (_haveBullet <= 0) //남은 탄환 없음
                {
                    Debug.Log("No Bullet");
                    return;
                }
                if (Bullet == _maxBullet) //이미 꽉 참
                {
                    Debug.Log("It's full");
                    return;
                }
                Debug.Log("Reloading");
                if (!IsReloading)
                {
                    IsReloading = true;
                    Reload();
                }
                else return;
        }
    }

    void Shoot()
    {
            _gunEffect.Play();
            Fire(); //진짜 발사
            GetComponent<Animator>().SetTrigger("recoil"); //반동 애니
            Bullet--;
            Invoke("RecoilToIdleTrigger", 0.5f); //원래 상태로
    }
    void Fire()
    {
        RaycastHit Hit;
        if(Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out Hit, _range))
        {
            EnemyHealth target = Hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            HitEffect(Hit); // 맞은 곳 반짝
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
            IdleTrigger();
        }
        //뭘 해 야 할 까 ?
    }

    void Reload()
    {
            GetComponent<Animator>().SetTrigger("reload"); //장전 모션
            int AddBullet = _maxBullet - Bullet;
            if (AddBullet > _haveBullet)
            {
                Bullet += _haveBullet;
                _haveBullet = 0;
                return;
            }
            _haveBullet -= AddBullet;
            Bullet += AddBullet;
            Invoke("IdleTrigger", 1.5f); //원래 상태로
    }

    void HitEffect(RaycastHit hit)
    {
        GameObject Effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(Effect,0.1f);
    }

    void IdleTrigger()
    {
        GetComponent<Animator>().SetTrigger("idle");
        IsReloading = false;
        IsAiming = false;
    }

    void RecoilToIdleTrigger()
    {
        GetComponent<Animator>().SetTrigger("idle");
    }

}
