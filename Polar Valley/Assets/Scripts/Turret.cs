using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{

    // Componentes de las torres
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject range;
    [SerializeField] private  AudioClip shootSound;

    // Atributos de las torres
    [Header("Attribute")]
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] public int baseUpgradeCost = 100;
    [SerializeField] private bool isShotgun = false;
    [SerializeField] private bool canAttackAir = false;

    private float bpsBase;
    private float targetingRangeBase;

    private AudioSource soundEffectSource;
    private Transform target;
    private float timeUntilFire;
    private int level = 1;
    private int sellCurrency;
    public int currentUpgradeCost = 0;

    private void Start()
    {
        bpsBase = bulletsPerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradeTurret);
        Tower turret = BuildingManager.main.GetSelectedTower();
        sellCurrency = (turret.cost * 75) / 100;
        soundEffectSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bulletsPerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (isShotgun)
        {
            ShootBullet(Vector2.zero);
            ShootBullet(new Vector2(0, 0.1f));
            ShootBullet(new Vector2(0, -0.1f));
        }
        else
        {
            ShootBullet(Vector2.zero);
        }

        if (shootSound != null && soundEffectSource != null)
        {
            soundEffectSource.PlayOneShot(shootSound);
        }
    }

    private void ShootBullet(Vector2 offset)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObject.GetComponent<Bullet>();
        bulletScript.SetSpeed(bulletSpeed);
        bulletScript.SetDamage(bulletDamage);
        Vector2 direction = ((Vector2)(target.position - firingPoint.position)).normalized + offset;
        bulletScript.SetDirection(direction.normalized);
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        foreach (var hit in hits)
        {
            var enemy = hit.transform.GetComponent<EnemyStats>();
            var boss = hit.transform.GetComponent<BossEnemy>();

            if (canAttackAir)
            {
                if (enemy != null && enemy.IsAirEnemy())
                {
                    target = hit.transform;
                    return;
                }
            }
            else
            {
                if (enemy != null && !enemy.IsAirEnemy())
                {
                    target = hit.transform;
                    return;
                }
                else if (boss != null)
                {
                    target = hit.transform;
                    return;
                }
            }
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    public void OpenUpgradeUI()
    {
        if (!BuildingManager.main.sellMode)
        {
            upgradeUI.SetActive(true);
            AdjustRangeSize();
            range.SetActive(true);
            UIManager.main.SetRange(range);
            CalculateCost();
            UpgradeUIHandler.main.UpdateUpgradeCost(this);
            

        }
        else
        {
            Destroy(gameObject);
            LevelManager.main.IncreaseCurrency(sellCurrency);
        }
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
        AdjustRangeSize();
        range.SetActive(false);
    }

    public void UpgradeTurret()
    {
        if (CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        bulletsPerSecond = CalculateBPS();
        targetingRange = CalculateRange();

        AdjustRangeSize();

        CloseUpgradeUI();
    }

    private int CalculateCost()
    {
        currentUpgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
        return currentUpgradeCost;
    }

    private float CalculateBPS()
    {
        return bulletsPerSecond * Mathf.Pow(level, 0.3f);
    }

    private float CalculateRange()
    {
        return targetingRange * Mathf.Pow(level, 0.3f);
    }

    private void OnDrawGizmosSelected()
    {
        /*Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);*/
    }

    private void AdjustRangeSize()
    {
        float diameter = targetingRange * 2f;
        range.transform.localScale = new Vector3(diameter, diameter, 1f);
    }
}
