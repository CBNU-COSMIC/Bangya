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
    [SerializeField] float _fireRate = 0.2f;  // �߻� ����
    [SerializeField] int _bullet = 10; //Ȯ�ο�
    bool IsAiming = false;
    bool IsReloading = false;
    float NextFireTime = 0f; //���� �߻�

    void Start()
    {
        _bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButton("Fire1")&&Time.time>=NextFireTime) //�߻�
        {
            if (_bullet <= 0) //�Ѿ� ����
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
                if (_bullet == _maxBullet) //�̹� �� ��
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
        _player.GetComponent<Animator>().SetTrigger("Shoot"); //�ݵ� �ִ�
        _bullet--;
        Invoke("ShootToIdleTrigger", 0.5f); //���� ���·�
        NextFireTime = Time.time +_fireRate;
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
            _player.GetComponent<Animator>().SetTrigger("Reload"); //���� ���
            int AddBullet = _maxBullet - _bullet;
            if (AddBullet > _haveBullet)
            {
                _bullet += _haveBullet;
                _haveBullet = 0;
                return;
            }
            _haveBullet -= AddBullet;
            _bullet += AddBullet;
            Invoke("IdleTrigger", 2.4f); //���� ���·�
    }

    void HitEffect(RaycastHit hit)
    {
        GameObject Effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(Effect,0.5f);
    }

    void IdleTrigger()
    {
        _player.GetComponent<Animator>().SetTrigger("Idle");
        IsReloading = false;
        IsAiming = false;
    }

    void ShootToIdleTrigger()
    {
        _player.GetComponent<Animator>().SetTrigger("Idle");
    }

}
