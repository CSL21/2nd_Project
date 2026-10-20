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
        SwapWeapon(0);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwapWeapon(0);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwapWeapon(1);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SwapWeapon(2);
        }
    }

    private void SwapWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;
        if (PlayerStatus.instance == null) return;

        if (index == 1 && !PlayerStatus.instance.isMultiEnabled) return;
        if (index == 2 && !PlayerStatus.instance.isPierceEnabled) return;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == index);
            }
        }

        currentWeaponIndex = index;
    }
}