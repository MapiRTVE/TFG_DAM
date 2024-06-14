using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que gestiona el movimiento de los enemigos
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isBoss = false;


    private EnemyStats enemyStats;
    private BossEnemy bossStats;
    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = LevelManager.main.path[pathIndex];

        // Verificar si el enemigo es un jefe y obtener las estadísticas correspondientes
        if (isBoss)
        {
            bossStats = GetComponent<BossEnemy>();
        }
        else
        {
            enemyStats = GetComponent<EnemyStats>(); // Obtener referencia al script EnemyStats
        }
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex == LevelManager.main.path.Length)
            {
                PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                if (playerHealth != null)
                {
                    if (isBoss)
                    {
                        playerHealth.TakeDamage(bossStats.damageToPlayer);
                    }
                    else
                    {
                        playerHealth.TakeDamage(enemyStats.damageToPlayer);
                    }
                }
                LevelManager.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        // Acceder a la velocidad del enemigo dependiendo si es un jefe o no
        float moveSpeed = isBoss ? bossStats.GetCurrentSpeed() : enemyStats.GetCurrentSpeed();
        rb.velocity = direction * moveSpeed;
    }

    public void SetTargetIndex(int index)
    {
        pathIndex = index;
        target = LevelManager.main.path[pathIndex];
    }

    public int PathIndex
    {
        get { return pathIndex; }
    }

}
