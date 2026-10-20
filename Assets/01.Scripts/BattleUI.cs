using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI potionCountText;
    [SerializeField] private TextMeshProUGUI bombCountText;

    [SerializeField] private GameObject multiXImage;
    [SerializeField] private GameObject pierceXImage;

    private void Start()
    {
        UpdateTexts();
    }

    private void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        if (PlayerStatus.instance == null) return;

        if (healthText != null)
            healthText.text = $"HP: {PlayerStatus.instance.currentHealth}";

        if (potionCountText != null)
            potionCountText.text = $"Potion x {PlayerStatus.instance.Potion}";

        if (bombCountText != null)
            bombCountText.text = $"Bomb x {PlayerStatus.instance.Bomb}";

        if (multiXImage != null)
            multiXImage.SetActive(!PlayerStatus.instance.isMultiEnabled);

        if (pierceXImage != null)
            pierceXImage.SetActive(!PlayerStatus.instance.isPierceEnabled);
    }
}