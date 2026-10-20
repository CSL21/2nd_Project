using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{

    [SerializeField] private string poolObjectName;
    [SerializeField] float speed = 5f;
    [SerializeField] float totalDamage = 0f;
    [SerializeField] float arrowDamage = 10f;
    [SerializeField] private bool isPiercing = false;
    [SerializeField] private int pierceCount = 1;

    [SerializeField] float lifeTime = 1f;

    private int currentPierceCount;
    float timer;

    Rigidbody2D rb;

    private List<GameObject> hitEnemies = new List<GameObject>();


    private void Awake()
    {
        poolObjectName = gameObject.name;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = 0f;

        currentPierceCount = pierceCount;

        hitEnemies.Clear();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= lifeTime)
        {
            ReturnPool();
        }
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        rb.linearVelocity = transform.right * speed;
    }

    public void SetDamage()
    {
        totalDamage = arrowDamage + PlayerStatus.instance.BaseDamage;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.layer==6) // Wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ReturnPool();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (hitEnemies.Contains(collision.gameObject)) return;

            hitEnemies.Add(collision.gameObject);

            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(totalDamage);
                Debug.Log($"µ¥¹ÌÁö {totalDamage} ");
            }

            BossController boss = collision.gameObject.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(totalDamage);
            }


            if (isPiercing)
            {
                currentPierceCount--;
                if (currentPierceCount <= 0)
                {
                    ReturnPool();
                }
            }
            else
            {
                ReturnPool();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy_Projectile"))
        {
            if (hitEnemies.Contains(collision.gameObject)) return;

            hitEnemies.Add(collision.gameObject);

            EnemyProjectile enemyProjectile = collision.gameObject.GetComponent<EnemyProjectile>();
            if (enemyProjectile != null)
            {
                enemyProjectile.TakeDamage(totalDamage);
            }

            if (isPiercing)
            {
                currentPierceCount--;
                if (currentPierceCount <= 0)
                {
                    ReturnPool();
                }
            }
            else
            {
                ReturnPool();
            }
        }

    }

    void ReturnPool()
    {
        SoundManager.instance.PlaySFX(SFXType.Arrow);
        ObjectPoolManager.instance.ReturnObject(poolObjectName, this.gameObject);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene Scene, LoadSceneMode mode)
    {
        if (Scene.name == "LobbyScene" || Scene.name == "MainScene")
        {
            ObjectPoolManager.instance.ReturnObject(poolObjectName, this.gameObject);
        }
    }
}
