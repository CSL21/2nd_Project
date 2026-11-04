using UnityEngine;
using UnityEngine.InputSystem;

public class Basic : Weapon
{
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject projectile;

    [Header("Auto Fire Settings")]
    [SerializeField] private float fireRate = 0.5f;
    private float nextAttackTime = 0f;

    public void Configure(Transform firePos, GameObject projectilePrefab)
    {
        firePosition = firePos;
        projectile = projectilePrefab;
    }

    protected override void Attack()
    {
        float currentAttackSpeed = (PlayerStatus.instance != null) ? PlayerStatus.instance.AttackSpeed : 1.0f;
        if (currentAttackSpeed <= 0f) currentAttackSpeed = 1.0f;

        float finalCooldown = fireRate / currentAttackSpeed;

        if (Mouse.current.leftButton.isPressed && canAttack && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + finalCooldown;

            string poolName = projectile != null ? projectile.name : "Arrow";
            GameObject arr = ObjectPoolManager.instance.GetObject(poolName);
            if (arr != null)
            {
                arr.transform.position = firePosition.position;
                arr.transform.rotation = transform.rotation;
                arr.GetComponent<Arrow>().SetDamage();
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
    }

    void Update()
    {
        LookMouse();
        Attack();
    }
}
