using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxHP = 100;
    [SerializeField] float nowHP;

    float moveSpeed;

    Transform target;

    SpriteRenderer sr;

    [SerializeField] float range = 100f;

    HPBar hpBar;

    [SerializeField] int touchDamage = 10;
    [SerializeField] float damageCooldown = 0.5f;
    private float lastDamageTime;

    [SerializeField] float traceRange = 15f;
    [SerializeField] float attackRange = 7f;

    [SerializeField] float RangedattackRange = 5f;
    [SerializeField] float RangedattackCooldown = 2f;
    [SerializeField] private string projectilePoolName;
    private float lastAttackTime;

    private void OnEnable()
    {
        nowHP = maxHP;
    }

    void Start()
    {
        moveSpeed = 2f;
        sr = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.Find("Player");
        target = playerObj.transform;
    }

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= range)
        {
            Trace();

            if (distance <= RangedattackRange)
            {
                    TryProjectileAttack();
            }
        }
    }

    void TryProjectileAttack()
    {
        if (Time.time < lastAttackTime + RangedattackCooldown) return;

        GameObject projectile = ObjectPoolManager.instance.GetObject(projectilePoolName);
        if (projectile == null) return;

        projectile.transform.position = transform.position;

        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        lastAttackTime = Time.time;
    }

    void Trace()
    {

        sr.flipX = CheckFlip();
        
        Move();
    }

    void Move()
    {

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    bool CheckFlip()
    {
        return transform.position.x > target.position.x ? true : false;
    }

    public void TakeDamage(float damage)
    {
        nowHP -= damage;

        if (hpBar != null)
        {
            hpBar.SetGauge(nowHP / maxHP);
        }


        if (nowHP <= 0)
        {
            nowHP = 0;
            Die();
        }
    }

    public void EnemyHpBar(HPBar bar)
    {
        hpBar = bar;
        if (hpBar != null)
        {
            hpBar.SetTarget(this.transform);
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time < lastDamageTime + damageCooldown)
            return;

        PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();
        if (PlayerStatus.instance != null)
        {
            PlayerStatus.instance.TakeDamage(touchDamage);
            lastDamageTime = Time.time;
        }
    }
}