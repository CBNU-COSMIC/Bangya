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
    [SerializeField] float _damage = 30f; //�� ����
    [SerializeField] float _range = 30f;  //�� ����
    [SerializeField] int _maxBullet = 10; //�ִ� źȯ ��
    [SerializeField] int _haveBullet = 100; //������ �ִ� źȯ ��
    int Bullet = 10; //�ʱⰪ
    bool IsAiming = false;
    bool IsReloading = false;

    void Start()
    {
        Bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //�߻�
        {
            if (Bullet <= 0) //�Ѿ� ����
            {
                Debug.Log("No Bullet");
                return;
            }
            if (!IsReloading) Shoot(); //�߻�
            else return;
        }

        else if (Input.GetButtonDown("Fire2")) Aim();  //����

        else if (Input.GetKeyDown(KeyCode.R)) //����
            {
                if (_haveBullet <= 0) //���� źȯ ����
                {
                    Debug.Log("No Bullet");
                    return;
                }
                if (Bullet == _maxBullet) //�̹� �� ��
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
            Fire(); //��¥ �߻�
            GetComponent<Animator>().SetTrigger("recoil"); //�ݵ� �ִ�
            Bullet--;
            Invoke("RecoilToIdleTrigger", 0.5f); //���� ���·�
    }
    void Fire()
    {
        RaycastHit Hit;
        if(Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out Hit, _range))
        {
            EnemyHealth target = Hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            HitEffect(Hit); // ���� �� ��¦
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
        //�� �� �� �� �� ?
    }

    void Reload()
    {
            GetComponent<Animator>().SetTrigger("reload"); //���� ���
            int AddBullet = _maxBullet - Bullet;
            if (AddBullet > _haveBullet)
            {
                Bullet += _haveBullet;
                _haveBullet = 0;
                return;
            }
            _haveBullet -= AddBullet;
            Bullet += AddBullet;
            Invoke("IdleTrigger", 1.5f); //���� ���·�
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
