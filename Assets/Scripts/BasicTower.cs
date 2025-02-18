using UnityEngine;

public class BasicTower : BaseTower
{
    public override void Shoot(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (target.position - transform.position).normalized;
        projectile.GetComponent<Projectile>().SetDirection(direction);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        detectionRadius += 0.5f;
        fireRate *= 1.2f;
    }
}