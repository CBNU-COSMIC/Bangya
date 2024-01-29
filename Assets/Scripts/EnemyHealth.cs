using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float _hitPoint = 100f;
    Animator anim;

    public void TakeDamage(float damage)
    {
        _hitPoint -= damage;
        if (_hitPoint > 0) anim.Play("GetHit", 0, 0);
        else if (_hitPoint <= 0)
        {
            anim.Play("Die", 0, 0);
            Invoke("DestroyEnemy", 3f);
        }
    }

    private void DestroyEnemy()
    {
        // Destroy the game object after 3 seconds
        Destroy(gameObject);
    }
}
