using UnityEngine;
using TMPro;

public class GoldManagerHandler : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI goldText;

    void OnEnable() => GameClient.Instance.OnDataChanged += HandleDataChanged;
    void OnDisable() => GameClient.Instance.OnDataChanged -= HandleDataChanged;

    private void HandleDataChanged(DataCategory category, CharacterData data) {
        if (category != DataCategory.GOLD && category != DataCategory.ALL) {
            return;
        }
        goldText.text = $"Gold: {data.gold}";
    }
}