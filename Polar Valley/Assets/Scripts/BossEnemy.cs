using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Attributes")]
    [SerializeField] public int health = 2;
    [SerializeField] private int currencyWorth = 50;
    [SerializeField] private int scoreWorth = 50;
    [SerializeField] private float speed = 5f;
    [SerializeField] public int damageToPlayer = 10;
    [SerializeField] private Sprite[] healthSprites; // Sprites de salud reducida
    [SerializeField] private GameObject spriteObject; // Objeto que contiene el sprite interior
    [SerializeField] private GameObject enemyObject; // Objeto que contiene el sprite del enemigo

    public bool isDestroyed = false;
    private SpriteRenderer healthSpriteRenderer; // Referencia al componente SpriteRenderer del sprite interior
    private SpriteRenderer enemySpriteRenderer; // Referencia al componente SpriteRenderer del enemigo
    private float currentSpeed;
    private Coroutine freezeCoroutine;
    private Coroutine burnCoroutine;
    private Coroutine bossCoroutine;
    private Color originalColor; // Color original del sprite del enemigo
    private bool bossIsAlive = false;
    private float maxHealth;


    private void Start()
    {
        maxHealth = health;
        healthSpriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        enemySpriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        currentSpeed = speed;
        originalColor = enemySpriteRenderer.color; // Almacenar el color original del sprite del enemigo

        bossIsAlive = true;
        bossCoroutine = StartCoroutine(BossActionRoutine());
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Cambiar el sprite según la cantidad de salud restante
        if (health <= 0 && !isDestroyed)
        {
            LevelManager.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            LevelManager.main.IncreaseScore(scoreWorth);
            isDestroyed = true;
            bossIsAlive = false; // Detiene la corutina cuando el jefe es destruido
            if (bossCoroutine != null)
            {
                StopCoroutine(bossCoroutine);
            }
            Destroy(gameObject);
        }
        else if (healthSprites != null && healthSprites.Length > 0)
        {
            // Calcular el porcentaje de salud
            float healthPercentage = (float)health / maxHealth;

            // Determinar el sprite a usar basado en el porcentaje de salud
            int spriteIndex = 0;
            if (healthPercentage <= 0)
            {
                spriteIndex = 0; // Vida 0%
            }
            else if (healthPercentage <= 0.25f)
            {
                spriteIndex = 1; // Vida 25%
            }
            else if (healthPercentage <= 0.5f)
            {
                spriteIndex = 2; // Vida 50%
            }
            else if (healthPercentage <= 0.75f)
            {
                spriteIndex = 3; // Vida 75%
            }
            else
            {
                spriteIndex = 4; // Vida 100%
            }

            // Asegurarse de que el índice del sprite está dentro de los límites del array
            spriteIndex = Mathf.Clamp(spriteIndex, 0, healthSprites.Length - 1);

            // Asignar el sprite correspondiente
            healthSpriteRenderer.sprite = healthSprites[spriteIndex];
        }
    }

    public IEnumerator Freeze(float duration)
    {
        SetSpeed(speed / 2); // Reduce la velocidad a la mitad
        enemySpriteRenderer.color = Color.blue; // Cambiar el color del sprite del enemigo a azul
        yield return new WaitForSeconds(duration);
        RestoreSpeed();
    }

    public IEnumerator Burn(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            // Aplicar efecto de daño cada segundo mientras está quemado
            TakeDamage(3); // Daño cada segundo
                           // Cambiar el color del sprite del enemigo a naranja
            enemySpriteRenderer.color = new Color(1f, 0.5f, 0f, 1f); // Color naranja
            yield return new WaitForSeconds(1f);
        }
        // Restaurar cualquier efecto de quemadura aplicado
        enemySpriteRenderer.color = originalColor; // Restaurar el color original del sprite del enemigo
    }

    private IEnumerator BossActionRoutine()
    {
        while (bossIsAlive)
        {
            SetSpeed(0); // Detener al jefe
            yield return new WaitForSeconds(0.5f); // Esperar un segundo mientras el jefe está detenido

            EnemyMovement bossMovement = GetComponent<EnemyMovement>();
            if (bossMovement != null)
            {
                int bossPathIndex = bossMovement.PathIndex; // Obtener el índice de la ruta del jefe
                GenerateEnemy(bossPathIndex); // Generar un enemigo usando el índice de la ruta del jefe
            }

            yield return new WaitForSeconds(0.5f);

            SetSpeed(speed); // Volver a mover al jefe
            yield return new WaitForSeconds(2f); // Esperar dos segundos antes de continuar
        }
    }


    public void RestoreSpeed()
    {
        SetSpeed(speed);
        enemySpriteRenderer.color = originalColor; // Restaurar el color original del sprite del enemigo
    }

    public void ApplyFreeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }
        freezeCoroutine = StartCoroutine(Freeze(duration));
    }

    public void ApplyBurn(float duration)
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        burnCoroutine = StartCoroutine(Burn(duration));
    }

    public void GenerateEnemy(int bossPathIndex)
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex];

        // Generar el enemigo en la posición del jefe
        GameObject enemy = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        LevelManager.main.BossEnemySpawn();

        // Configurar la posición inicial del enemigo
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            // Establecer el objetivo inicial usando el mismo pathIndex que el jefe
            enemyMovement.SetTargetIndex(bossPathIndex);
        }
    }


}
