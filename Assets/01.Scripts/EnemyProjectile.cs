using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Enemy Projectile Data")]
    [SerializeField] private string poolObjectName;
    [SerializeField] float speed = 5f;
    [SerializeField] int EnemyProjectileDamage = 10;
    [SerializeField] float lifeTime = 1f;
    [SerializeField] float ProjectileHealth;

    float timer;

    Rigidbody2D rb;

    private void Awake()
    {
        poolObjectName = gameObject.name;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

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

    private void LateUpdate()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (transform.right.x < 0)
            {
                sr.flipY = true;
            }
            else
            {
                sr.flipY = false;
            }
        }
    }

    public void SetDamage()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.layer==6) // Wall
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ReturnPool();
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            if (PlayerStatus.instance != null)
            {
                {
                   PlayerStatus.instance.TakeDamage(EnemyProjectileDamage);
                }
                ReturnPool();
            }
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Barricade"))
        {
            BarricadeHealth barricade = collision.gameObject.GetComponent<BarricadeHealth>();

            if (barricade != null)
            {
                barricade.TakeDamage(EnemyProjectileDamage);
            }
            ReturnPool();
        }
    }

    public void TakeDamage(float damage)
    {
        ProjectileHealth -= damage;

        if (ProjectileHealth <= 0)
        {
            ReturnPool();
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
