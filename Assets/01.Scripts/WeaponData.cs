using UnityEngine;
public enum ItemType
{
    Weapon,
    Potion,
    Bomb
}

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Shop Item")]
public class ShopItemData : ScriptableObject
{
    [Header("공통 정보")]
    public string itemName;
    public int price;
    public Sprite itemIcon;
    public ItemType itemType;

    [Header("무기 전용")]
    public GameObject weaponPrefab;

    [Header("소모품 전용")]
    public int maxCarryLimit = 5;
}