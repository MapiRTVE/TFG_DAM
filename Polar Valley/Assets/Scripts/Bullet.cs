using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private bool isPiercing = false;
    [SerializeField] private bool isFire = false;
    [SerializeField] private bool isIce = false;
    [SerializeField] private bool isAir = false;

    private Transform target;
    private int bulletDamage = 1;
    private float burnDuration = 3f; // Duración de la quemadura

    private void Start()
    {
        if (rb.velocity == Vector2.zero && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }

        Invoke("DestroyBullet", 5f);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }

    //Metodo para balas normales 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isPiercing)
        {
            HandleCollision(collision.collider);
        }
    }

    //Metodo para balas perforantes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPiercing)
        {
            HandleCollision(collision);
        }
    }

    //Metodo que se encarga de manejar las colisiones de las balas aplicando efectos especiales
    private void HandleCollision(Collider2D collider)
    {
        EnemyStats enemy = collider.GetComponent<EnemyStats>();
        BossEnemy boss = collider.GetComponent<BossEnemy>();

        if (enemy != null)
        {

            if (enemy.IsAirEnemy() && !isAir)
            {
                return;
            }
            else if (!enemy.IsAirEnemy() && isAir)
            {
                return;
            }

            enemy.TakeDamage(bulletDamage);

            if (isFire)
            {
                BurnEnemy(enemy);
            }
            else if (isIce)
            {
                FreezeEnemy(enemy);
            }

            if (!isPiercing)
            {
                DestroyBullet();
            }
        }
        else if (boss != null){
            boss.TakeDamage(bulletDamage);

            if (isFire)
            {
                BurnBoss(boss);
            }
            else if (isIce)
            {
                FreezeBoss(boss);
            }

            if (!isPiercing)
            {
                DestroyBullet();
            }
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void SetSpeed(float bulletSpeed)
    {
        this.bulletSpeed = bulletSpeed;
    }

    public void SetDamage(int bulletDamage)
    {
        this.bulletDamage = bulletDamage;
    }

    private void FreezeEnemy(EnemyStats enemy)
    {
        if (enemy != null)
        {
            enemy.ApplyFreeze(3f);
        }
    }

    private void BurnEnemy(EnemyStats enemy)
    {
        if (enemy != null)
        {
            enemy.ApplyBurn(burnDuration);
        }
    }

    private void FreezeBoss(BossEnemy boss)
    {
        if (boss != null)
        {
            boss.ApplyFreeze(3f);
        }
    }

    private void BurnBoss(BossEnemy boss)
    {
        if (boss != null)
        {
            boss.ApplyBurn(burnDuration);
        }
    }
}
