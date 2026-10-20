using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<string> enemyPoolNames = new List<string>() { "Enemy_Goblin" };
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemyCount = 10;

    [Header("Spawn Position")]
    [SerializeField] private float spawnXPosition = 25f;

    [SerializeField] private List<float> fixedYPositions = new List<float>() { -4.2f };
    [SerializeField] private float maxSpawnPlayerX = 70f;

    private Transform playerTransform;
    private bool isSpawning = false;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        StartSpawn();
    }

    public void StartSpawn()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawn()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform == null) continue;

            if (playerTransform.position.x >= maxSpawnPlayerX)
            {
                Debug.Log($"맵 끝 부근에 도달하여 스폰을 중단합니다.");
                continue;
            }


            int currentEnemyCount = CountAliveEnemies();

            if (currentEnemyCount >= maxEnemyCount)
            {
                Debug.Log($"몬스터의 숫자가 한계에 도달했습니다.");
                continue;
            }

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (playerTransform == null) return;
        if (enemyPoolNames.Count == 0 || fixedYPositions.Count == 0) return;

        int randomEnemyIndex = Random.Range(0, enemyPoolNames.Count);
        string selectedEnemyPool = enemyPoolNames[randomEnemyIndex];

        GameObject enemy = ObjectPoolManager.instance.GetObject(selectedEnemyPool);

        if (enemy != null)
        {
            float calculatedX = playerTransform.position.x + spawnXPosition;

            int randomYIndex = Random.Range(0, fixedYPositions.Count);
            float selectedYPosition = fixedYPositions[randomYIndex];

            Vector3 spawnPos = new Vector3(calculatedX, selectedYPosition, 0f);

            enemy.transform.position = spawnPos;
        }
    }


    private int CountAliveEnemies()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (enemyLayer == -1)
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Length;
        }

        EnemyController[] currentEnemies = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        return currentEnemies.Length;
    }
}