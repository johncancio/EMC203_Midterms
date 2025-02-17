using UnityEngine;

public class MultishotTower : BaseTower
{
    public int bulletCount = 3;

    public override void Shoot(Transform target)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i;
            Vector3 direction = (target.position - transform.position).normalized;
            direction = Quaternion.Euler(0f, 0f, angle) * direction;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        detectionRadius += 0.5f;
        if (bulletCount < 8)
        {
            bulletCount++;
        }
    }
}

