using UnityEngine;
using UnityEngine.UI;

public class GoldDrop : MonoBehaviour
{
    public float speed = 5f;
    private Transform targetUI;
    private int goldValue;

    public void Initialize(Transform uiTarget, int value)
    {
        targetUI = uiTarget;
        goldValue = value;
    }

    void Update()
    {
        if (targetUI != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetUI.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetUI.position) < 0.1f)
            {
                FindFirstObjectByType<GameManager>().AddGold(goldValue);
                Destroy(gameObject);
            }
        }
    }
}