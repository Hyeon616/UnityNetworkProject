using UnityEngine;
using System.Collections.Generic;

public class WeaponInventoryUIManager : MonoBehaviour
{
    public static WeaponInventoryUIManager Instance { get; private set; }

    public List<WeaponInventorySlot> slots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        // 초기 슬롯 상태를 설정합니다.
        foreach (var slot in slots)
        {
            slot.Initialize(slot.Rarity, slot.Grade);
        }
    }

    public void UpdateWeaponSlots(List<Weapon> weapons)
    {
        // 모든 슬롯을 초기화합니다.
        foreach (var slot in slots)
        {
            slot.SetSlot(null, false);
        }

        // 무기에 맞는 슬롯을 업데이트합니다.
        foreach (var weapon in weapons)
        {
            foreach (var slot in slots)
            {
                if (slot.Rarity.ToString() == weapon.rarity && slot.Grade.ToString() == weapon.grade)
                {
                    slot.SetSlot(weapon, weapon.count > 0);
                    break; // 이 슬롯은 업데이트 되었으므로 다음 슬롯으로 이동합니다.
                }
            }
        }
    }
}
