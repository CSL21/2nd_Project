using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Burst : Weapon
{
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject Arrow;

    [Header("Burst Settings")]
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstInterval = 0.08f;

    [Header("Auto Fire Settings")]
    [SerializeField] private float fireRate = 0.5f;
    private float nextAttackTime = 0f;

    private bool isBursting = false;

    private void OnEnable()
    {
        isBursting = false;
    }

    private void OnDisable()
    {
        isBursting = false;
    }

    protected override void Attack()
    {
        float currentAttackSpeed = (PlayerStatus.instance != null) ? PlayerStatus.instance.AttackSpeed : 1.0f;

        if (currentAttackSpeed <= 0) currentAttackSpeed = 1.0f;

        float finalCooldown = fireRate / currentAttackSpeed;

        if (Mouse.current.leftButton.isPressed && canAttack && !isBursting && Time.time >= nextAttackTime)
        {
            StartCoroutine(BurstFireRoutine(finalCooldown, currentAttackSpeed));
        }
    }

    private IEnumerator BurstFireRoutine(float finalCooldown, float currentAttackSpeed)
    {
        isBursting = true;

        nextAttackTime = Time.time + finalCooldown;

        for (int i = 0; i < burstCount; i++)
        {
            GameObject arr = ObjectPoolManager.instance.GetObject("Arrow");
            if (arr != null)
            {
                arr.transform.position = firePosition.position;
                arr.transform.rotation = transform.rotation;
                arr.GetComponent<Arrow>().SetDamage();
            }

            if (i < burstCount - 1)
            {
                float finalInterval = burstInterval / currentAttackSpeed;
                yield return new WaitForSeconds(finalInterval);
            }
        }

        isBursting = false;
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