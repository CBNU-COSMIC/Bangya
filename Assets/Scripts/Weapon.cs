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
    [SerializeField] float _fireRate = 0.2f;  // 발사 간격
    [SerializeField] int _bullet = 10; //확인용
    bool IsAiming = false;
    bool IsReloading = false;
    float NextFireTime = 0f; //다음 발사

    void Start()
    {
        _bullet = _maxBullet;
    }

    void Update()
    {
        if (Input.GetButton("Fire1")&&Time.time>=NextFireTime) //발사
        {
            if (_bullet <= 0) //총알 없음
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
                if (_bullet == _maxBullet) //이미 꽉 참
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
        _player.GetComponent<Animator>().SetTrigger("Shoot"); //반동 애니
        _bullet--;
        Invoke("ShootToIdleTrigger", 0.5f); //원래 상태로
        NextFireTime = Time.time +_fireRate;
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
            _player.GetComponent<Animator>().SetTrigger("Reload"); //장전 모션
            int AddBullet = _maxBullet - _bullet;
            if (AddBullet > _haveBullet)
            {
                _bullet += _haveBullet;
                _haveBullet = 0;
                return;
            }
            _haveBullet -= AddBullet;
            _bullet += AddBullet;
            Invoke("IdleTrigger", 2.4f); //원래 상태로
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
