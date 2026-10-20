using UnityEngine;

public class BarricadeHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public float maxHealth = 200f;
    public float currentHealth;

    public static BarricadeHealth instance;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            DestroyBarricade();
        }
    }

    private void DestroyBarricade()
    {
        Destroy(gameObject);
    }
}