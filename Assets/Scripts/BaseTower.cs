using UnityEngine;
using System.Linq;

public abstract class BaseTower : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float detectionRadius = 3f;
    public float fireRate = 1f;
    public int towerLevel = 1;
    public int maxLevel = 5;
    public float rotationSpeed = 600f;

    private float fireCooldown = 0f;
    private Transform currentTarget;

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

    public abstract void Shoot(Transform target);

    public virtual void Upgrade()
    {
        if (towerLevel >= maxLevel)
        {
            Debug.Log(gameObject.name + " is already at max level!");
            return;
        }
        if (GameManager.Instance != null && GameManager.Instance.SpendGold(100))
        {
            towerLevel++;
            detectionRadius += 0.5f;
            fireRate += 0.2f;
            Debug.Log(gameObject.name + " upgraded to level " + towerLevel);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade " + gameObject.name);
        }
    }
}