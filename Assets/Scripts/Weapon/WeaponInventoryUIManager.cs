using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryUIManager : Singleton<WeaponInventoryUIManager>
{
    [SerializeField] private List<WeaponInventorySlot> weaponSlots;
    [SerializeField] private Button synthesizeAllButton;
    [SerializeField] private GameObject combineButtonPrefab; // 합성 버튼 프리팹
    private GameObject combineButtonInstance;
    private WeaponInventorySlot selectedSlot;

    private Dictionary<int, WeaponInventorySlot> slotDictionary = new Dictionary<int, WeaponInventorySlot>();

    protected override void Awake()
    {
        base.Awake();
        InitializeSlotDictionary();
    }

    private void OnEnable()
    {
        synthesizeAllButton.onClick.AddListener(OnSynthesizeAllButtonPressed);
    }

    private void OnDisable()
    {
        synthesizeAllButton.onClick.RemoveListener(OnSynthesizeAllButtonPressed);
    }

    private void InitializeSlotDictionary()
    {
        slotDictionary.Clear(); // 딕셔너리 초기화

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            var slot = weaponSlots[i];
            if (i < 32) // 슬롯의 개수가 무기 개수보다 적은 경우를 방지
            {
                slot.SetSlot(new Weapon { id = i + 1 }, false); // 무기 ID를 설정
                Debug.Log($"Initializing slot. SlotName: {slot.SlotName}, WeaponId: {slot.WeaponId}");
            }

            if (slot.WeaponId != 0) // 무기 ID가 0이 아닌 경우만 추가
            {
                if (slotDictionary.ContainsKey(slot.WeaponId))
                {
                    Debug.LogWarning($"Duplicate WeaponId found: {slot.WeaponId} in WeaponInventorySlot list, SlotName: {slot.SlotName}");
                }
                else
                {
                    slotDictionary[slot.WeaponId] = slot;
                    Debug.Log($"Added WeaponId {slot.WeaponId} to slotDictionary, SlotName: {slot.SlotName}");
                }
            }
            else
            {
                Debug.LogWarning($"WeaponId is 0 for a slot in the WeaponInventorySlot list, SlotName: {slot.SlotName}");
            }
        }
    }

    public void UpdateWeaponSlots(List<Weapon> weapons)
    {
        foreach (var weapon in weapons)
        {
            if (slotDictionary.TryGetValue(weapon.id, out var slot))
            {
                slot.SetSlot(weapon, true);
                Debug.Log($"Updated slot for Weapon ID: {weapon.id}");
            }
            else
            {
                Debug.LogWarning($"No slot found for Weapon ID: {weapon.id}");
            }
        }
    }

    public void IncreaseWeaponCount(Weapon weapon)
    {
        if (slotDictionary.TryGetValue(weapon.id, out var slot))
        {
            slot.IncreaseCount();
            Debug.Log($"Increased count for Weapon ID: {weapon.id}, New Count: {slot.Count}");
        }
        else
        {
            Debug.LogError($"No slot found for Weapon ID: {weapon.id}");
        }
    }

    public void ActivateWeaponSlot(Weapon weapon)
    {
        if (slotDictionary.TryGetValue(weapon.id, out var slot))
        {
            slot.SetSlot(weapon, true);
            Debug.Log($"Activated slot for Weapon ID: {weapon.id}");
        }
    }

    public void ShowCombineButton(WeaponInventorySlot slot)
    {
        if (selectedSlot != null && selectedSlot != slot)
        {
            selectedSlot.ResetSlotState(); // 이전 선택된 슬롯 상태 복구
        }

        if (combineButtonInstance != null)
        {
            combineButtonInstance.SetActive(false); // 비활성화
        }

        selectedSlot = slot;
        combineButtonInstance = Instantiate(combineButtonPrefab, slot.transform);
        combineButtonInstance.transform.localPosition = Vector3.zero; // 슬롯 정중앙에 버튼 위치 설정

        var combineButton = combineButtonInstance.GetComponent<Button>();
        combineButton.onClick.AddListener(OnCombineButtonClicked);

        // 버튼 상태 업데이트
        slot.UpdateCombineButtonState();
    }

    private void OnCombineButtonClicked()
    {
        if (selectedSlot != null && selectedSlot.Count >= 5)
        {
            OnSynthesizeButtonPressed(selectedSlot.WeaponId);
            combineButtonInstance.SetActive(false); // 합성 후 버튼 비활성화
        }
    }

    public void OnSynthesizeButtonPressed(int weaponId)
    {
        SynthesizeWeaponAsync(weaponId).Forget();
    }

    private async UniTask SynthesizeWeaponAsync(int weaponId)
    {
        bool success = await WeaponManager.Instance.SynthesizeWeapon(weaponId);
        if (success)
        {
            await WeaponManager.Instance.FetchWeapons();
            UpdateWeaponSlots(WeaponManager.Instance.GetActiveWeapons()); // UI 업데이트
        }
        else
        {
            Debug.LogError("Failed to synthesize weapon.");
        }
    }

    public void OnSynthesizeAllButtonPressed()
    {
        SynthesizeAllWeaponsAsync().Forget();
    }

    private async UniTask SynthesizeAllWeaponsAsync()
    {
        bool success = await WeaponManager.Instance.SynthesizeAllWeapons();
        if (success)
        {
            await WeaponManager.Instance.FetchWeapons();
            UpdateWeaponSlots(WeaponManager.Instance.GetActiveWeapons()); // UI 업데이트
        }
        else
        {
            Debug.LogError("Failed to synthesize all weapons.");
        }
    }
}
