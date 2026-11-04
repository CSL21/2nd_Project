using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Extends the original project with five authored stages and an endless defense mode.
/// Difficulty changes enemy count, spawn tempo, and health only. Enemy attack values stay untouched.
/// </summary>
public static class GameModeBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Register()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int stage = ParseStage(scene.name);
        if (stage > 0)
        {
            new GameObject("StageDifficulty").AddComponent<StageDifficulty>().Configure(stage, false);
            return;
        }

        if (scene.name == "DefenseScene")
        {
            SpawnManager legacySpawner = UnityEngine.Object.FindFirstObjectByType<SpawnManager>();
            if (legacySpawner != null) legacySpawner.enabled = false;

            GameObject host = new GameObject("DefenseMode");
            StageDifficulty difficulty = host.AddComponent<StageDifficulty>();
            difficulty.Configure(5, true);
            host.AddComponent<DefenseMode>();
        }
    }

    private static int ParseStage(string sceneName)
    {
        const string prefix = "StageScene";
        if (!sceneName.StartsWith(prefix, StringComparison.Ordinal)) return 0;
        return int.TryParse(sceneName.Substring(prefix.Length), out int number)
            ? Mathf.Clamp(number, 1, 5)
            : 0;
    }
}

public class StageDifficulty : MonoBehaviour
{
    private static readonly float[] HealthScale = { 1f, 1.30f, 1.70f, 2.15f, 2.70f };
    private static readonly float[] CountScale = { 1f, 1.25f, 1.55f, 1.90f, 2.30f };

    private readonly HashSet<int> scaledEnemyIds = new HashSet<int>();
    private float healthScale;
    private bool defenseMode;

    public void Configure(int stage, bool isDefense)
    {
        int index = Mathf.Clamp(stage, 1, 5) - 1;
        healthScale = HealthScale[index];
        defenseMode = isDefense;

        if (!defenseMode)
        {
            SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
            if (spawner != null)
            {
                spawner.ApplyStageDifficulty(CountScale[index], index);
            }
        }
    }

    public void UpdateDefenseHealthScale(float newScale)
    {
        healthScale = newScale;
    }

    private void Update()
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (EnemyController enemy in enemies)
        {
            int id = enemy.GetInstanceID();
            if (scaledEnemyIds.Contains(id)) continue;

            enemy.ApplyHealthScale(healthScale);
            scaledEnemyIds.Add(id);

            if (defenseMode && enemy.GetComponent<DefenseEnemyBrain>() == null)
            {
                enemy.gameObject.AddComponent<DefenseEnemyBrain>();
            }
        }

        scaledEnemyIds.RemoveWhere(id => !EnemyStillActive(id, enemies));
    }

    private static bool EnemyStillActive(int id, EnemyController[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetInstanceID() == id) return true;
        }
        return false;
    }
}

public sealed class DefenseMode : MonoBehaviour
{
    private readonly string[] enemyPools = { "Enemy_Goblin", "Enemy_Lizard", "Enemy_Orc" };

    // 플레이어 체력 정보 및 상태를 확인할 스크립트 레퍼런스
    private PlayerStatus playerStatus;
    private StageDifficulty difficulty;

    private float gameElapsedTime;
    private int difficultyStage = 0;
    private float nextSpawnTime;
    private int totalSpawnedCount;

    private bool failed;
    private float returnAt;

    private IEnumerator Start()
    {
        GoalObject goal = FindFirstObjectByType<GoalObject>();
        if (goal != null) goal.gameObject.SetActive(false);

        // 하이어라키에서 플레이어의 Status 스크립트 검색
        playerStatus = FindFirstObjectByType<PlayerStatus>();
        difficulty = GetComponent<StageDifficulty>();

        while (ObjectPoolManager.instance == null) yield return null;

        nextSpawnTime = Time.time + 1.0f;
    }

    private void Update()
    {
        if (failed)
        {
            if (Time.time >= returnAt) SceneManager.LoadScene("LobbyScene");
            return;
        }

        // [실패 조건 체크]
        // 1. 플레이어 스크립트가 통째로 지워졌거나 파괴되었을 때
        // 2. 혹은 PlayerStatus 내부의 HP가 0 이하가 되었을 때 (※ 본인 스크립트의 HP 변수명에 맞게 .hp 등으로 수정하세요)
        if (playerStatus == null || playerStatus.currentHealth <= 0)
        {
            failed = true;
            returnAt = Time.time + 2.5f;
            return;
        }

        // 30초 주기 난이도 계산
        gameElapsedTime += Time.deltaTime;
        int currentStage = Mathf.FloorToInt(gameElapsedTime / 30f);

        if (currentStage > difficultyStage)
        {
            difficultyStage = currentStage;
        }

        // 30초마다 체력 스케일링 +0.1
        float currentHealthScale = 2.70f + (difficultyStage * 0.1f);
        difficulty.UpdateDefenseHealthScale(currentHealthScale);

        // 30초마다 스폰 주기 0.1초씩 단축 (최소 0.15초 제한)
        float spawnInterval = Mathf.Max(0.15f, 1.0f - (difficultyStage * 0.1f));

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            totalSpawnedCount++;
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolManager.instance.GetObject(enemyPools[UnityEngine.Random.Range(0, enemyPools.Length)]);
        if (enemy != null)
        {
            Vector3 spawnPos = new Vector3(20f + UnityEngine.Random.Range(0f, 5f), -4.2f, 0f);
            if (playerStatus != null)
            {
                // 플레이어 머리끄덩이(위치) 기준으로 우측에서 스폰되도록 처리
                spawnPos.x = playerStatus.transform.position.x + 15f + UnityEngine.Random.Range(0f, 5f);
            }
            enemy.transform.position = spawnPos;
        }
    }

    private static int CountEnemies() => FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

    private void OnGUI()
    {
        if (failed)
        {
            GUI.Box(new Rect(Screen.width / 2f - 160f, Screen.height / 2f - 40f, 320f, 80f), "DEFENSE FAILED\nReturning to lobby...");
        }
        else
        {
            string timeStr = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(gameElapsedTime / 60f), Mathf.FloorToInt(gameElapsedTime % 60f));
            GUI.Box(new Rect(12f, 12f, 280f, 65f), $"ENDLESS DEFENSE\nTime: {timeStr} (Level {difficultyStage + 1})\nSpawned: {totalSpawnedCount} | Alive: {CountEnemies()}");
        }
    }
}

public sealed class DefenseEnemyBrain : MonoBehaviour
{
    private PlayerStatus playerStatusTarget;
    private float nextAttack;

    private void Start()
    {
        // 적들도 플레이어의 위치를 쫓아가기 위해 Status 검색
        playerStatusTarget = FindFirstObjectByType<PlayerStatus>();
    }

    private void Update()
    {
        if (playerStatusTarget == null) return;

        // 플레이어의 위치를 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, playerStatusTarget.transform.position, 2f * Time.deltaTime);

        // 사정거리 안으로 들어왔을 때의 공격 처리
        if (Vector2.Distance(transform.position, playerStatusTarget.transform.position) <= 0.9f && Time.time >= nextAttack)
        {

            nextAttack = Time.time + 0.75f;
        }
    }
}