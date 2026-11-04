using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponSwapManager : MonoBehaviour
{
    [Header("Weapon List")]
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();

    private int currentWeaponIndex = 0;

    private void Start()
    {
        EnsureFiveWeapons();
        SwapWeapon(0);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SwapWeapon(0);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) SwapWeapon(1);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) SwapWeapon(2);
        else if (Keyboard.current.digit4Key.wasPressedThisFrame) SwapWeapon(3);
        else if (Keyboard.current.digit5Key.wasPressedThisFrame) SwapWeapon(4);
    }

    private void EnsureFiveWeapons()
    {
        if (weapons == null || weapons.Count == 0) return;
        if (weapons.Count >= 5) return;
        if (weapons.Count != 3) return;

        // Legacy layout: [Burst/Bow, Multi/Knife, Pierce/Hammer]
        GameObject burstWeapon = weapons[0];
        GameObject multiWeapon = weapons[1];
        GameObject pierceWeapon = weapons[2];

        Transform firePos = null;
        if (burstWeapon.transform.parent != null)
            firePos = burstWeapon.transform.parent.Find("PlayerHand");
        if (firePos == null)
            firePos = burstWeapon.transform;

        GameObject basicWeapon = CreateRuntimeWeapon(burstWeapon, "BasicWeapon");
        basicWeapon.AddComponent<Basic>().Configure(firePos, null);

        GameObject rapidWeapon = CreateRuntimeWeapon(burstWeapon, "RapidWeapon");
        rapidWeapon.AddComponent<Rapid>().Configure(firePos, null);

        weapons = new List<GameObject>
        {
            basicWeapon,
            multiWeapon,
            pierceWeapon,
            burstWeapon,
            rapidWeapon
        };
    }

    private static GameObject CreateRuntimeWeapon(GameObject template, string name)
    {
        GameObject weapon = new GameObject(name);
        weapon.transform.SetParent(template.transform.parent, false);
        weapon.transform.localPosition = template.transform.localPosition;
        weapon.transform.localRotation = template.transform.localRotation;
        weapon.transform.localScale = template.transform.localScale;

        SpriteRenderer templateRenderer = template.GetComponent<SpriteRenderer>();
        if (templateRenderer != null)
        {
            SpriteRenderer sr = weapon.AddComponent<SpriteRenderer>();
            sr.sprite = templateRenderer.sprite;
            sr.color = templateRenderer.color;
            sr.sortingLayerID = templateRenderer.sortingLayerID;
            sr.sortingOrder = templateRenderer.sortingOrder;
            sr.sharedMaterial = templateRenderer.sharedMaterial;
        }

        weapon.SetActive(false);
        return weapon;
    }

    private void SwapWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;
        if (PlayerStatus.instance == null) return;

        if (index == 1 && !PlayerStatus.instance.isMultiEnabled) return;
        if (index == 2 && !PlayerStatus.instance.isPierceEnabled) return;
        if (index == 4 && !PlayerStatus.instance.isRapidEnabled) return;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
                weapons[i].SetActive(i == index);
        }

        currentWeaponIndex = index;
    }
}
