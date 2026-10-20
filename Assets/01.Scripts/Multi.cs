using UnityEngine;
using UnityEngine.InputSystem;

public class Multi : Weapon
{
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject projectile;

    [Header("Multi Shot Settings")]
    [SerializeField] private int arrowCount = 3;
    [SerializeField] private float spreadAngle = 15f;

    [Header("Auto Fire Settings")]
    [SerializeField] private float fireRate = 0.5f;
    private float nextAttackTime = 0f;

    protected override void Attack()
    {
        float currentAttackSpeed = (PlayerStatus.instance != null) ? PlayerStatus.instance.AttackSpeed : 1.0f;

        if (currentAttackSpeed <= 0) currentAttackSpeed = 1.0f;

        float finalCooldown = fireRate / currentAttackSpeed;

        if (Mouse.current.leftButton.isPressed && canAttack && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + finalCooldown;

            float startAngle = -((arrowCount - 1) * spreadAngle) / 2f;

            for (int i = 0; i < arrowCount; i++)
            {
                GameObject arr = ObjectPoolManager.instance.GetObject(projectile.name);
                if (arr != null)
                {
                    arr.transform.position = firePosition.position;
                    float currentSpreadAngle = startAngle + (i * spreadAngle);
                    Quaternion spreadRotation = transform.rotation * Quaternion.Euler(0, 0, currentSpreadAngle);
                    arr.transform.rotation = spreadRotation;
                    arr.GetComponent<Arrow>().SetDamage();
                }
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