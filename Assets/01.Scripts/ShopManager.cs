using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private int multiWeaponPrice = 300;
    [SerializeField] private int pierceWeaponPrice = 500;
    [SerializeField] private int rapidWeaponPrice = 400;
    [SerializeField] private int bombWeaponPrice = 100;
    [SerializeField] private int potionPrice = 50;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void RefreshLobbyUI()
    {
        UIManager uiManager = Object.FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateLobbyUI();
        }
    }

    public void BuyBasicWeapon()
    {
        // Basic is the free starter weapon (key 1).
    }

    public void BuyMultiWeapon()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.Gold >= multiWeaponPrice)
        {
            PlayerStatus.instance.Gold -= multiWeaponPrice;
            PlayerStatus.instance.isMultiEnabled = true;
            RefreshLobbyUI();
        }
    }

    public void BuyPierceWeapon()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.Gold >= pierceWeaponPrice)
        {
            PlayerStatus.instance.Gold -= pierceWeaponPrice;
            PlayerStatus.instance.isPierceEnabled = true;
            RefreshLobbyUI();
        }
    }

    public void BuyRapidWeapon()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.Gold >= rapidWeaponPrice)
        {
            PlayerStatus.instance.Gold -= rapidWeaponPrice;
            PlayerStatus.instance.isRapidEnabled = true;
            RefreshLobbyUI();
        }
    }

    public void BuyBombWeapon()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.Gold >= bombWeaponPrice)
        {
            PlayerStatus.instance.Gold -= bombWeaponPrice;
            PlayerStatus.instance.Bomb++;
            RefreshLobbyUI();
        }
    }

    public void BuyPotion()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.Gold >= potionPrice)
        {
            PlayerStatus.instance.Gold -= potionPrice;
            PlayerStatus.instance.Potion++;
            RefreshLobbyUI();
        }
    }
}