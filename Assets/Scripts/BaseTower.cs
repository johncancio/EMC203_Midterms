using UnityEngine;
using System.Linq;

public abstract class BaseTower : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float detectionRadius = 3f;
    public float fireRate = 1f;
    public int towerLevel = 1;
    public int maxLevel = 5;
    public float rotationSpeed = 200f; // Speed of turret rotation

    private float fireCooldown = 0f;
    private Transform currentTarget; // Store the current target

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        FindClosestEnemy();

        if (currentTarget != null)
        {
            RotateTowardsTarget(); // Rotate before shooting
            if (fireCooldown <= 0f)
            {
                Shoot(currentTarget);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    // Find the closest enemy in range
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentTarget = enemies
            .Where(e => (e.transform.position - transform.position).magnitude <= detectionRadius)
            .OrderBy(e => (e.transform.position - transform.position).magnitude)
            .Select(e => e.transform)
            .FirstOrDefault();
    }

    // Rotate towards the target enemy
    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Apply rotation to face the enemy
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Abstract Shoot function for specific towers
    public abstract void Shoot(Transform target);

    // Upgrade tower logic
    public virtual void Upgrade()
    {
        if (towerLevel < maxLevel)
        {
            towerLevel++;
            detectionRadius += 0.5f;
            fireRate += 0.2f;
            Debug.Log(gameObject.name + " upgraded to level " + towerLevel);
        }
    }
}