using UnityEngine;

public class BarricadeHealth : MonoBehaviour
{
    [Header("ü�� ����")]
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
        if (instance == this)
        {
            instance = null;
        }

        Destroy(gameObject);
    }
}