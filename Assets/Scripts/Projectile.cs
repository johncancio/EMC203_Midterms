using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if ((enemy.transform.position - transform.position).magnitude < 0.3f)
            {
                Destroy(enemy);
                Destroy(gameObject);
                return;
            }
        }
    }
}