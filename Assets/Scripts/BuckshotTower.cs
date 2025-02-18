using UnityEngine;
using TMPro;
using System.Linq;

public class BuckshotTower : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float detectionRadius = 3f;
    public float fireRate = 1f;
    public int towerLevel = 1;
    public int maxLevel = 5;
    public float rotationSpeed = 600f;
    public int bulletCount = 3;
    public float spreadAngle = 10f;
    public TMP_Text levelText;

    private float fireCooldown = 0f;
    private Transform currentTarget;

    private void Start()
    {
        updateLevelUI();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        FindClosestEnemy();

        if (currentTarget != null)
        {
            RotateTowardsTarget();
            if (fireCooldown <= 0f)
            {
                Shoot(currentTarget);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentTarget = enemies.Where(e => (e.transform.position - transform.position).magnitude <= detectionRadius)
                               .OrderBy(e => (e.transform.position - transform.position).magnitude)
                               .Select(e => e.transform)
                               .FirstOrDefault();
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;
        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void updateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Lvl " + towerLevel;
        }
    }

    public void Shoot(Transform target)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = -spreadAngle / 2 + i * (spreadAngle / (bulletCount - 1));
            Vector3 direction = (target.position - transform.position).normalized;
            direction = Quaternion.Euler(0f, 0f, angle) * direction;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }

    public void Upgrade()
    {
        if (towerLevel >= maxLevel)
        {
            Debug.Log(gameObject.name + " is already at max level!");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.SpendGold(100))
        {
            towerLevel++; // Increase level only after spending gold
            spreadAngle += 0.5f;

            if (towerLevel == 3 || towerLevel == 5)
            {
                bulletCount++;
            }

            updateLevelUI();
            Debug.Log(gameObject.name + " upgraded to level " + towerLevel);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade " + gameObject.name);
        }
    }
}
