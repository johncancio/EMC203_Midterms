using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerHP = 10;
    public TMP_Text hpText;
    public TMP_Text waveText;
    public TMP_Text gameOverText;
    public Transform cameraTransform;
    public float cameraShakeIntensity = 0.2f;
    public float cameraShakeDuration = 0.5f;

    public TMP_Text goldText;
    public GameObject goldPrefab;
    public Transform goldUI;
    private int currentGold = 0;
    private int displayedGold = 0;

    private Vector3 originalCameraPos;
    private int waveNumber = 1;
    private int enemiesRemaining = 0;
    private const int maxWaves = 10;
    private bool waveInProgress = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        originalCameraPos = cameraTransform.position;
        UpdateHPUI();
        UpdateGoldUI();
        UpdateWaveUI();
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        StartCoroutine(StartWaveWithDelay());
    }

    public void EnemyReachedEnd()
    {
        playerHP--;
        UpdateHPUI();
        StartCoroutine(CameraShake());

        enemiesRemaining--;
        if (playerHP <= 0)
        {
            GameOver();
        }
        else if (enemiesRemaining <= 0 && waveInProgress)
        {
            waveInProgress = false;
            StartNextWave();
        }
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0 && waveInProgress)
        {
            waveInProgress = false;
            StartNextWave();
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        StartCoroutine(AnimateGoldUI());
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            StartCoroutine(AnimateGoldUI());
            return true;
        }
        return false;
    }

    IEnumerator AnimateGoldUI()
    {
        int startValue = displayedGold;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            displayedGold = Mathf.RoundToInt(Mathf.Lerp(startValue, currentGold, elapsedTime / duration));
            goldText.text = displayedGold.ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayedGold = currentGold;
        goldText.text = displayedGold.ToString();
    }

    void UpdateHPUI()
    {
        hpText.text = "HP: " + playerHP;
    }

    void UpdateGoldUI()
    {
        goldText.text = currentGold.ToString();
    }

    void UpdateWaveUI()
    {
        waveText.text = "Wave: " + waveNumber + "/" + maxWaves;
    }

    IEnumerator CameraShake()
    {
        float elapsed = 0f;
        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-0.2f, 0.2f) * cameraShakeIntensity;
            float y = Random.Range(-0.2f, 0.2f) * cameraShakeIntensity;
            cameraTransform.position = originalCameraPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraTransform.position = originalCameraPos;
    }

    void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over!";
            gameOverText.enabled = true;
        }
        StartCoroutine(ReloadSceneAfterDelay(2f));
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartNextWave()
    {
        if (!waveInProgress && waveNumber <= maxWaves)
        {
            StartCoroutine(StartWaveWithDelay());
        }
    }

    IEnumerator StartWaveWithDelay()
    {
        yield return new WaitForSeconds(3f);
        if (waveNumber <= maxWaves)
        {
            waveInProgress = true;
            int enemyCount = Mathf.FloorToInt(5 * Mathf.Pow(1.2f, waveNumber));
            enemiesRemaining = enemyCount;
            UpdateWaveUI();
            FindFirstObjectByType<EnemySpawner>().StartWave(enemyCount);
            waveNumber++;
        }
        else
        {
            GameOver();
        }
    }
}
