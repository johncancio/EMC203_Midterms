using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int playerHP = 10;
    public TMP_Text hpText;
    public GameObject gameOverScreen;
    public Transform cameraTransform;
    public float cameraShakeIntensity = 0.2f;
    public float cameraShakeDuration = 0.5f;

    private Vector3 originalCameraPos;
    private int waveNumber = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        originalCameraPos = cameraTransform.position;
        UpdateHPUI();
        StartNextWave();
    }

    public void EnemyReachedEnd()
    {
        playerHP--;
        UpdateHPUI();
        StartCoroutine(CameraShake());

        enemiesRemaining--; // Reduce enemy count when they reach the end
        CheckWaveCompletion();

        if (playerHP <= 0)
        {
            GameOver();
        }
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--; // Reduce enemy count when an enemy is destroyed
        CheckWaveCompletion();
    }

    void UpdateHPUI()
    {
        hpText.text = "HP: " + playerHP;
    }

    IEnumerator CameraShake()
    {
        float elapsed = 0f;
        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-0.2f, 0.2f) * cameraShakeIntensity;
            float y = Random.Range(-0.2f, 0.2f) * cameraShakeIntensity;
            cameraTransform.position += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraTransform.position = originalCameraPos;
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void StartNextWave()
    {
        waveNumber++;
        int enemyCount = Mathf.FloorToInt(5 * Mathf.Pow(1.2f, waveNumber));

        enemiesRemaining = enemyCount; // Track the number of enemies in this wave
        FindFirstObjectByType<EnemySpawner>().StartWave(enemyCount);
    }

    void CheckWaveCompletion()
    {
        if (enemiesRemaining <= 0)
        {
            StartNextWave();
        }
    }
}