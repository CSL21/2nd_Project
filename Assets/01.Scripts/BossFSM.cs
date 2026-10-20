using UnityEngine;

// 보스의 명확한 행동 상태 분리
public enum BossState { Trace, TripleShot, Dash }

public class BossController : MonoBehaviour
{
    [SerializeField] float maxHP;
    [SerializeField] float nowHP;
    [SerializeField] float moveSpeed;

    [SerializeField] float tripleShotCooldown = 5f;
    [SerializeField] float dashCooldown = 8f;
    private float lastTripleShotTime;
    private float lastDashTime;

    [SerializeField] int touchDamage = 30;
    [SerializeField] float knockbackForce = 15f;

    [SerializeField] private string projectilePoolName;
    [SerializeField] float burstDelay = 0.5f;

    [SerializeField] float dashSpeed = 12f;
    [SerializeField] float dashDuration = 0.6f;
    private Vector2 dashDirection;

    private BossState currentState = BossState.Trace;
    private Transform target;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private HPBar hpBar;

    private void OnEnable()
    {
        nowHP = maxHP;
        currentState = BossState.Trace;
        lastTripleShotTime = Time.time;
        lastDashTime = Time.time;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) target = playerObj.transform;
    }

    void Update()
    {
        if (target == null || currentState == BossState.Dash) return;

        if (currentState == BossState.Trace)
        {
            if (Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(TriggerDashPattern());
                return;
            }
            if (Time.time >= lastTripleShotTime + tripleShotCooldown)
            {
                StartCoroutine(TriggerTripleShotPattern());
                return;
            }

            HandleTrace();
        }
    }

    void HandleTrace()
    {
        sr.flipX = transform.position.x > target.position.x;
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    System.Collections.IEnumerator TriggerTripleShotPattern()
    {
        currentState = BossState.TripleShot;
        lastTripleShotTime = Time.time;

        for (int i = 0; i < 3; i++)
        {
            sr.flipX = transform.position.x > target.position.x;

            GameObject proj = ObjectPoolManager.instance.GetObject(projectilePoolName);
            if (proj != null)
            {
                proj.transform.position = transform.position;
                Vector2 dir = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                proj.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            yield return new WaitForSeconds(burstDelay);
        }
        currentState = BossState.Trace;
    }

    System.Collections.IEnumerator TriggerDashPattern()
    {
        currentState = BossState.Dash;
        lastDashTime = Time.time;

        sr.flipX = transform.position.x > target.position.x;
        dashDirection = (target.position - transform.position).normalized;
        yield return new WaitForSeconds(0.5f);

        float dashTimer = 0f;
        while (dashTimer < dashDuration)
        {
            dashTimer += Time.deltaTime;
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        currentState = BossState.Trace;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (PlayerStatus.instance != null && playerRb != null)
        {
            PlayerStatus.instance.TakeDamage(touchDamage);

            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(pushDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        nowHP -= damage;

        if (nowHP <= 0)
        {
            nowHP = 0;
            gameObject.SetActive(false);
        }
    }
}