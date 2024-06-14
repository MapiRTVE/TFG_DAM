using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject musicManager;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip winSound;

    [Header("Wave Configurations")]
    [SerializeField] private List<WaveConfig> waves;

    [Header("Attributes")]
    [SerializeField] public int currency = 0;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private long expPerMinute = 0;
    [SerializeField] private string level_id = "";

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public Transform startPoint;
    public Transform[] path;

    public int score;
    private bool isSpawning = false;
    public int currentWave = 0;
    private float timeSinceLastSpawn;
    public int enemiesAlive;
    private int enemiesLeftToSpawn;
    private List<EnemyType> waveEnemies = new List<EnemyType>();
    private DateTime startTime;
    private int spentMoney;
    private AudioSource audioSource;
    private bool isBossWave = false;

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        score = 0;
        spentMoney = 0;
        startTime = DateTime.Now;
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            // Volver a la música normal
            if (isBossWave) {
                audioSource.clip = music;
                isBossWave = false;
            }
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemy()
    {
        if (currentWave >= waves.Count)
        {
            Debug.Log("All waves completed!");
            return;
        }

        EnemyType enemy = null;

        for (int x = 0; x < waveEnemies.Count; x++)
        {
            EnemyType possibleEnemy = waveEnemies[x];

            if (possibleEnemy.remainEnemy > 0)
            {
                enemy = possibleEnemy;
            }
            else
            {
                waveEnemies.Remove(possibleEnemy);
            }
        }

        if (enemy != null)
        {
            // Si es un jefe, cambiar la música a la de jefe
            if (enemy.enemyPrefab.name.Contains("Boss")) {
                audioSource = musicManager.GetComponent<AudioSource>();
                audioSource.clip = bossMusic;
                audioSource.Play();
                isBossWave = true;
            }
            
            Instantiate(enemy.enemyPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
            enemy.reduceEnemies();
        } else
        {
            enemiesLeftToSpawn = 0;
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;

        if (currentWave < waves.Count)
        {
            if (waves[currentWave].enemies.Count > 0)
            {
                WaveConfig config = waves[currentWave];

                // Copiamos la lista de enemigos
                waveEnemies = new List<EnemyType>(config.enemies); 
                waveEnemies.Reverse();

                enemiesLeftToSpawn = 0;

                for (int x = 0; x < waveEnemies.Count; x++)
                {
                    enemiesLeftToSpawn += waveEnemies[x].remainEnemy;
                }
            }
            else
            {
                Debug.LogError("No enemy counts specified for the current wave.");
                isSpawning = false;
                yield break;
            }
           
        }
        else
        {
            Debug.Log("No more waves configured.");
            isSpawning = false;
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        if (currentWave < waves.Count)
        {
            StartCoroutine(StartWave());
        }
        else
        {
            Debug.Log("All waves completed!");
            UIManager.main.WinLevel();
            audioSource.PlayOneShot(winSound);
            //SaveData();
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        Menu.main.ToggleTurretButtons();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            spentMoney += amount;
            Menu.main.ToggleTurretButtons();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckCurrency(int amount)
    {
        if (amount <= currency)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IncreaseScore(int amount) {
        score += amount;
    }

    public void BossEnemySpawn()
    {
        enemiesAlive++;
    }

    private void SaveData()
    {
        TimeSpan duration = DateTime.Now - startTime;

        RealmManager.SaveGameData(new GameData
        {
            Id = ObjectId.GenerateNewId(),
            UserId = RealmManager.realmUser.Id,
            GameDate = DateTimeOffset.Now,
            GameDuration = (long) duration.TotalSeconds,
            MaxWave = waves.Count,
            SpendMoney = spentMoney,
            LevelId = level_id
        });

        long minutes = (long) duration.TotalMinutes;
        long givenExp = minutes * expPerMinute;

        GamePlayer player = UserManager.main.getCurrentPlayer();
        UserManager.main.AddExperience(player,givenExp);
    }
}

[System.Serializable]
public class WaveConfig
{
    public List<EnemyType> enemies;
   
}

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int remainEnemy;

    public void reduceEnemies()
    {
        remainEnemy--;
    }
}