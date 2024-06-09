using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    public Text WeaponRarityText;
    public Text WeaponLevelText;
    public Text WeaponGradeText;
    public Text WeaponCountText;
    public Image ItemImage;
    public Image SlotBackgroundImage;
  //  public Button combineButton;


    private bool _hasBeenAcquired;
    public int Count { get; private set; }
    public int WeaponId { get; private set; }
    public string SlotName => gameObject.name;


    private Button button;

    private void Awake()
    {
        button = gameObject.AddComponent<Button>(); // Button 컴포넌트를 추가합니다.
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnSlotClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnSlotClicked);
    }


    public void Initialize(string rarity, string grade)
    {
        SetSlot(new Weapon { rarity = rarity, grade = grade, count = 0 }, false);
    }

    public void SetSlot(Weapon weapon, bool isActive)
    {
        if (weapon != null)
        {
            WeaponId = weapon.id; // 무기 ID 설정

            if (isActive)
            {
                WeaponRarityText.text = weapon.GetRarityName();
                WeaponGradeText.text = weapon.GetGradeName();
                WeaponCountText.text = $"{weapon.count}/5";
                WeaponLevelText.text = $"Lv. {weapon.level}";
                ItemImage.color = new Color(1f, 1f, 1f, 1f);
                SlotBackgroundImage.color = GetRarityColor(weapon.rarity);
                _hasBeenAcquired = true;
                Count = weapon.count;
            }
            else if (_hasBeenAcquired)
            {
                WeaponCountText.text = $"{weapon.count}/5";
                WeaponLevelText.text = $"Lv. {weapon.level}";
                ItemImage.color = new Color(1f, 1f, 1f, 1f);
                SlotBackgroundImage.color = GetRarityColor(weapon.rarity);
            }
            else
            {
                ItemImage.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
        else
        {
            ItemImage.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }

    private Color GetRarityColor(string rarity)
    {
        return rarity switch
        {
            "일반" => new Color(0.75f, 0.75f, 0.75f, 1f), // 밝은 회색
            "고급" => new Color(0f, 1f, 0f, 1f), // 초록
            "매직" => new Color(0f, 0f, 1f, 1f), // 파랑
            "유물" => new Color(1f, 0.65f, 0f, 1f), // 주황
            "영웅" => new Color(1f, 0f, 1f, 1f), // 마젠타
            "에픽" => new Color(1f, 1f, 0f, 1f), // 노랑
            "고대" => new Color(0.55f, 0f, 0f, 1f), // 빨강
            "신화" => new Color(0.53f, 0.81f, 0.92f, 1f), // 밝은 파랑
            _ => new Color(0f, 0f, 0f, 1f), // 기본 검정색
        };
    }

    public void IncreaseCount()
    {
        Count++;
        WeaponCountText.text = $"{Count}/5";
    }
    private void OnSlotClicked()
    {
        WeaponInventoryUIManager.Instance.ShowCombineButton(this);
    }

    public void UpdateCombineButtonState()
    {
        var combineButton = GetComponentInChildren<Button>();
        if (combineButton != null)
        {
            if (Count >= 5)
            {
                combineButton.interactable = true;
                var colors = combineButton.colors;
                colors.normalColor = Color.yellow;
                combineButton.colors = colors;
            }
            else
            {
                combineButton.interactable = false;
                var colors = combineButton.colors;
                colors.normalColor = Color.gray;
                combineButton.colors = colors;
            }
        }
    }

    public void ResetSlotState()
    {
        SetSlot(new Weapon { rarity = WeaponRarityText.text, grade = WeaponGradeText.text, count = Count }, _hasBeenAcquired);
    }



}
