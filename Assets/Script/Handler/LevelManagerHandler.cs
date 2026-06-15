using UnityEngine;
using TMPro;

public class LevelManagerHandler : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI leveltext;

    void OnEnable() => GameClient.Instance.OnDataChanged += HandleDataChanged;
    void OnDisable() => GameClient.Instance.OnDataChanged -= HandleDataChanged;

    private void HandleDataChanged(DataCategory category, CharacterData data) {
        if (category != DataCategory.LEVEL && category != DataCategory.ALL) {
            return;
        }
        leveltext.text = $": {data.level}";
    }
}