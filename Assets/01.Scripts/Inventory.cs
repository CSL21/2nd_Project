using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("무기 보관함")]
    public List<ShopItemData> ownedWeapons = new List<ShopItemData>();
    public ShopItemData equippedWeapon;

    public int potionCount;
    public int bombCount;

    [SerializeField] TextMeshProUGUI bombCountText;
    [SerializeField] TextMeshProUGUI potionCountText;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(ShopItemData item)
    {
        if (item == null) return;

        switch (item.itemType)
        {
            case ItemType.Weapon:
                if (!ownedWeapons.Contains(item))
                {
                    ownedWeapons.Add(item);
                    EquipWeapon(item);
                }
                break;

            case ItemType.Potion:
                potionCount++;
                break;

            case ItemType.Bomb:
                bombCount++;
                break;
        }
    }

    public void EquipWeapon(ShopItemData weaponItem)
    {
        if (weaponItem == null || weaponItem.itemType != ItemType.Weapon) return;

        equippedWeapon = weaponItem;

        if (PlayerStatus.instance != null)
        {
            PlayerStatus.instance.EquipWeaponPrefab(weaponItem.weaponPrefab);
        }
    }

    public void UpdateInventoryUI()
    {
        if (PlayerStatus.instance == null) return;

        if (bombCountText != null)
        {
            bombCountText.text = $"x{PlayerStatus.instance.Bomb}";
        }

        if (potionCountText != null)
        {
            potionCountText.text = $"x{PlayerStatus.instance.Potion}";
        }
    }
}