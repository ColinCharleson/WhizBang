using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyWaveSpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI enemiesRemainingText; 
    private int currentRound = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        StartNextRound();
    }

    void StartNextRound()
    {
        currentRound++;
        int enemiesToSpawn = 5 + (currentRound / 2) * 5; 
        enemiesRemaining = enemiesToSpawn;

        StartCoroutine(SpawnRound(enemiesToSpawn));
        UpdateRoundText();
    }

    IEnumerator SpawnRound(int enemiesToSpawn)
    {
        UpdateRemainingEnemiesText(); 

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();

        // Subscribe to the enemy's destruction event
        enemyAi.OnDeath += OnEnemyDeath;

    }

    void OnEnemyDeath()
    {
        enemiesRemaining -= 1;
        UpdateRemainingEnemiesText();
        Debug.Log(enemiesRemaining);

        if (enemiesRemaining == 0)
        {
            StartNextRound();
        }
    }

    void UpdateRoundText()
    {
        roundText.text = "Round: " + currentRound.ToString();
    }

    void UpdateRemainingEnemiesText()
    {
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining.ToString();
    }
}
