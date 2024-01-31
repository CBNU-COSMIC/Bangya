using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float _hitPoint=100f;

    public void TakeDamage(float damage)
    {
        _hitPoint -= damage;
        if (_hitPoint <= 0) Destroy(gameObject);
    }
}
