using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyWaveSpawn : MonoBehaviour
{
    public GameObject[] meleeEnemyPrefabs;
    public GameObject[] newEnemyPrefabs;
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
        GameObject player = GameObject.Find("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        // Set player's health to 100
        playerMovement.health = 100;

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

        GameObject enemy;

        if (currentRound <= 2)
        {
            // Spawn melee enemy for first two rounds
            GameObject randomMeleeEnemyPrefab = meleeEnemyPrefabs[Random.Range(0, meleeEnemyPrefabs.Length)];
            enemy = Instantiate(randomMeleeEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        }
        else
        {
            // After round 2, spawn new types of enemies
            GameObject randomEnemyPrefab = newEnemyPrefabs[Random.Range(0, newEnemyPrefabs.Length)];
            enemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        }

        // Add event listener for enemy death
        if (enemy.GetComponent<EnemyAi>())
            enemy.GetComponent<EnemyAi>().OnDeath += OnEnemyDeath;
        if (enemy.GetComponent<MeleeAI>())
            enemy.GetComponent<MeleeAI>().OnDeath += OnEnemyDeath;
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
