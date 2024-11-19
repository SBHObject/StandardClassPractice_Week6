using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UpgradeOption
{
    MaxHealth,
    AttackPower,
    Speed,
    Knockback,
    AttackDelay,
    NumberOfProjectiles,
    COUNT   //enum의 갯수 확인용
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    [SerializeField] private string playerTag = "Player";

    public ParticleSystem EffectParticle;

    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = 0.5f;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();

    [SerializeField] private List<GameObject> rewards = new List<GameObject>();

    [SerializeField] private CharacterStat defaultStats;
    [SerializeField] private CharacterStat rangedStats;

    public bool IsPaused { get; private set; } = false;
    public GameObject pauseUI;
    private void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();

        ObjectPool = GetComponent<ObjectPool>();

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        for(int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    private void Start()
    {
        UpgradeStatInit();

        StartCoroutine(StartNextWave());
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth/ playerHealthSystem.MaxHealth;
    }

    private void GameOver()    
    {
        gameOverUI.SetActive(true);
        StopAllCoroutines();
    }

    private void UpdateWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            if(currentSpawnCount == 0)
            {
                UpdateWaveUI();
                yield return new WaitForSeconds(2f);

                if(currentWaveIndex % 20 == 0)
                {
                    RandomUpgrade();
                }

                ProcessWaveConditions();

                yield return StartCoroutine(SpawnEnemiesInWave());
                currentWaveIndex++;
            }

            yield return null;
        }
    }

    private void ProcessWaveConditions()
    {
        if(currentWaveIndex % 20 == 0)
        {
            RandomUpgrade();
        }

        if(currentWaveIndex % 10 == 0)
        {
            IncreaseSpawnPositions();
            TakeRandomDebuff();

        }

        if(currentWaveIndex % 5 == 0)
        {
            CreateReward();
        }

        if(currentWaveIndex % 3 ==0)
        {
            IncreaseWaveSpawnCount();
        }
    }

    IEnumerator SpawnEnemiesInWave()
    {
        for(int i = 0; i < waveSpawnPosCount; i ++)
        {
            int posIdx = UnityEngine.Random.Range(0, spawnPositions.Count);
            for(int j = 0; j < waveSpawnCount; j ++)
            {
                SpawnEnemyAtPosition(posIdx);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnEnemyAtPosition(int posIdx)
    {
        int prefabIdx = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
        enemy.GetComponent<CharacterStatHandler>().AddStatModifire(defaultStats);
        enemy.GetComponent<CharacterStatHandler>().AddStatModifire(rangedStats);
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void IncreaseSpawnPositions()
    {
        waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
        waveSpawnCount = 0;
    }

    private void IncreaseWaveSpawnCount()
    {
        waveSpawnCount += 1;
    }

    private void UpgradeStatInit()
    {
        defaultStats.statsChangeType = StatsChangeType.Add;
        defaultStats.attackSO = Instantiate(defaultStats.attackSO);

        rangedStats.statsChangeType = StatsChangeType.Add;
        rangedStats.attackSO = Instantiate(rangedStats.attackSO);
    }

    private void RandomUpgrade()
    {
        UpgradeOption option = (UpgradeOption)UnityEngine.Random.Range(0, (int)UpgradeOption.COUNT);

        switch(option)
        {
            case UpgradeOption.MaxHealth:
                defaultStats.maxHealth += 2;
                break;

            case UpgradeOption.AttackPower:
                defaultStats.attackSO.power += 1;
                break;

            case UpgradeOption.Speed:
                defaultStats.speed += 0.1f;
                break;

            case UpgradeOption.Knockback:
                defaultStats.attackSO.isOnKnockback = true;
                defaultStats.attackSO.knockbackPower += 1;
                defaultStats.attackSO.knockbackTime = 0.1f;
                break;

            case UpgradeOption.AttackDelay:
                defaultStats.attackSO.delay -= 0.05f;
                break;

            case UpgradeOption.NumberOfProjectiles:
                RangedAttackSO rangedAttackData = rangedStats.attackSO as RangedAttackSO;
                if (rangedAttackData != null) rangedAttackData.numberofProjectilesPerShot += 1;
                break;

            default:
                break;
        }
    }

    private void CreateReward()
    {
        int selectedRewardIndex = UnityEngine.Random.Range(0, rewards.Count);
        int randomPositionIndex = UnityEngine.Random.Range(0, spawnPositions.Count);

        GameObject obj = rewards[selectedRewardIndex];
        Instantiate(obj, spawnPositions[randomPositionIndex].position, Quaternion.identity);
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    public void PauseGame()
    {
        if (IsPaused)
        {
            Time.timeScale = 1;
            IsPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            IsPaused = true;
        }
        pauseUI.SetActive(IsPaused);
    }

    private void TakeRandomDebuff()
    {
        int randomDamageRate = Random.Range(0, 51);
        float randomDamage = playerHealthSystem.CurrentHealth * (randomDamageRate / 100f);

        playerHealthSystem.TakeDamageNonInvincible(randomDamage);
    }
}
