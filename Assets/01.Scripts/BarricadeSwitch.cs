using UnityEngine;
using UnityEngine.InputSystem;

public class BarricadeSwitch : MonoBehaviour
{
    public GameObject targetBarricade;

    public SpriteRenderer spriteRenderer;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;

    private bool isActivated = false;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleSwitch();
        }
    }

    void ToggleSwitch()
    {
        isActivated = !isActivated;

        if (isActivated)
        {
            if (spriteRenderer != null && activatedSprite != null)
                spriteRenderer.sprite = activatedSprite;

            if (targetBarricade != null)
                targetBarricade.SetActive(true);
        }
        else
        {
            if (spriteRenderer != null && deactivatedSprite != null)
                spriteRenderer.sprite = deactivatedSprite;

            if (targetBarricade != null)
                targetBarricade.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}