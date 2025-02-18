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
        gameManager = FindFirstObjectByType<GameManager>();

        if (isCubic)
        {
            control1 = start + new Vector3(-6, -2, 0);
            control2 = start + new Vector3(4, -5, 0);
        }
        else
        {
            control1 = start + new Vector3(-8, 0, 0);
        }
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        float easedTime = TweenUtils.EaseInOut(t); // Using EaseInOut for smoother animation
        if (t >= 1)
        {
            gameManager.EnemyReachedEnd();
            Destroy(gameObject);
        }
        transform.position = isCubic ? CubicBezier(start, control1, control2, end, easedTime) : QuadraticBezier(start, control1, end, easedTime);
    }

    void OnDestroy()
    {
        GameObject gold = Instantiate(gameManager.goldPrefab, transform.position, Quaternion.identity);
        gold.GetComponent<GoldDrop>().Initialize(gameManager.goldUI.transform, 10);
        gameManager.EnemyDefeated();
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