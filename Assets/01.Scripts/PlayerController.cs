using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    private bool isSpeedBoosted = false;


    float dir;
    bool isGround;
    int jumpCount;
    int jumpCountMax;
    int isRun;
    int isJump;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        jumpCount = 0;
        jumpCountMax = 2;
        isRun = Animator.StringToHash("isRun");
        isJump = Animator.StringToHash("isJump");
    }

    void Update()
    {
        dir = 0;
        if (Keyboard.current.aKey.isPressed) dir += -1;
        if (Keyboard.current.dKey.isPressed) dir += 1;

        GroundCheck();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            UseBomb();
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            UsePotion();
        }

        Dash();
    }

    private void FixedUpdate()
    {
        if (dir != 0)
        {
            animator.SetBool(isRun, true);
            if (dir > 0)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
        else
        {
            animator.SetBool(isRun, false);
        }
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0.8f, groundLayer);
        isGround = hit.collider == null ? false : true;

        if (isGround)
        {
            jumpCount = 0;
            animator.SetBool(isJump, false);
        }
    }

    void Jump()
    {
        if (jumpCount >= jumpCountMax)
        {
            return;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        if (isGround) jumpCount++;
        else jumpCount += 2;

        animator.SetBool(isJump, true);
    }

    void Dash()
    {
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            if (!isSpeedBoosted)
            {
                moveSpeed = moveSpeed * 2f;
                isSpeedBoosted = true;
            }
        }
        else
        {
            if (isSpeedBoosted)
            {
                moveSpeed = moveSpeed / 2f;
                isSpeedBoosted = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.8f, 0), 0.3f);
    }

    private void UseBomb()
    {
        if (PlayerStatus.instance == null) return;
        if (PlayerStatus.instance.Bomb <= 0) return;

        PlayerStatus.instance.Bomb--;

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int projectileLayer = LayerMask.NameToLayer("Enemy_Projectile");

        Collider2D[] allColliders = Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None);

        foreach (Collider2D col in allColliders)
        {
            if (col.gameObject.layer == enemyLayer || col.gameObject.layer == projectileLayer)
            {
                col.gameObject.SendMessage("TakeDamage", 200, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void UsePotion()
    {
        if (PlayerStatus.instance == null)
            return;

        if (PlayerStatus.instance.Potion <= 0)
        {
            return;
        }



        PlayerStatus.instance.Potion--;
        PlayerStatus.instance.currentHealth += 50;

        if (PlayerStatus.instance.currentHealth > PlayerStatus.instance.maxHealth)
        {
            PlayerStatus.instance.currentHealth = PlayerStatus.instance.maxHealth;
        }
    }
}