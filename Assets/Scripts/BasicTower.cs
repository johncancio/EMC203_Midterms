using UnityEngine;
using TMPro;
using System.Linq;

public class BasicTower : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float detectionRadius = 3f;
    public float fireRate = 1f;
    public int towerLevel = 1;
    public int maxLevel = 5;
    public float rotationSpeed = 600f;
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (target.position - transform.position).normalized;
        projectile.GetComponent<Projectile>().SetDirection(direction);
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
            detectionRadius += 0.5f;
            fireRate *= 1.2f;
            updateLevelUI();
            Debug.Log(gameObject.name + " upgraded to level " + towerLevel);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade " + gameObject.name);
        }
    }
}
