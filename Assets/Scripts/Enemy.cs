using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 start, control1, control2, end;
    private float t = 0f;
    private bool isCubic;
    private GameManager gameManager;

    public void Initialize(bool cubic, Vector3 endPos)
    {
        isCubic = cubic;
        start = transform.position;
        end = endPos;
        control1 = start + new Vector3(2, 3, 0);
        control2 = start + new Vector3(-2, 3, 0);
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        if (t >= 1)
        {
            gameManager.EnemyReachedEnd();
            Destroy(gameObject);
        }
        transform.position = isCubic ? CubicBezier(start, control1, control2, end, t) : QuadraticBezier(start, control1, end, t);
    }

    private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return Mathf.Pow(1 - t, 2) * a + 2 * (1 - t) * t * b + t * t * c;
    }

    private Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        return Mathf.Pow(1 - t, 3) * a + 3 * Mathf.Pow(1 - t, 2) * t * b + 3 * (1 - t) * t * t * c + t * t * t * d;
    }
}
