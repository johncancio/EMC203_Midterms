using UnityEngine;

public class BuckshotTower : BaseTower
{
    public int bulletCount = 3;
    public float spreadAngle = 10f;

    public override void Shoot(Transform target)
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

    public override void Upgrade()
    {
        base.Upgrade();
        spreadAngle += 0.5f;

        if (towerLevel == 3 || towerLevel == 5)
        {
            bulletCount++;
        }
    }
}
