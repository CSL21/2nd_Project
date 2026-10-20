using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DummyController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.8f, 0), 0.3f);
    }


    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene")
        {
            gameObject.SetActive(false);
        }
        else if (scene.name == "MainScene")
        {
            gameObject.SetActive(true);

            transform.position = new Vector3(-6, -3, 0); 
        }
    }
}