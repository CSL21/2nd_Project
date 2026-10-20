using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI statPointText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI bombCountText;
    [SerializeField] private TextMeshProUGUI potionCountText;

    [SerializeField] private GameObject multiXImage;
    [SerializeField] private GameObject pierceXImage;

    [SerializeField] private GameObject invMultiXImage;
    [SerializeField] private GameObject invPierceXImage;

    public void UpdateLobbyUI()
    {
        {
            if (PlayerStatus.instance != null)
            {
                goldText.text = PlayerStatus.instance.Gold.ToString("N0");
                statPointText.text = "SP : " + PlayerStatus.instance.StatPoint.ToString();
                hpText.text = "HP : " + PlayerStatus.instance.currentHealth.ToString();
                damageText.text = "ATK : " + PlayerStatus.instance.BaseDamage.ToString("F1");
                speedText.text = "ATS : " + PlayerStatus.instance.AttackSpeed.ToString("F1");
                bombCountText.text = $"x{PlayerStatus.instance.Bomb}";
                potionCountText.text = $"x{PlayerStatus.instance.Potion}";

                multiXImage.SetActive(PlayerStatus.instance.isMultiEnabled);
                pierceXImage.SetActive(PlayerStatus.instance.isPierceEnabled);

                invMultiXImage.SetActive(!PlayerStatus.instance.isMultiEnabled);
                invPierceXImage.SetActive(!PlayerStatus.instance.isPierceEnabled);
            }
        }
    }
    void Start()
    {
        UpdateLobbyUI();
    }

}