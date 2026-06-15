using Game.Network;
using Game.Packets.Request;
using TMPro; // TMP를 사용하기 위해 필요합니다.
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour {
    public Slider hpSlider;

    public float maxHp = 100f;
    private float currentHp;

    public static MonsterController Instance {
        get; private set;
    }

    void Start() {
        Instance = this;
        currentHp = maxHp;
        hpSlider.value = 1f;
    }

    public void OnMonsterClicked() {
        currentHp -= 50f;
        hpSlider.value = currentHp / maxHp;

        if (currentHp <= 0) {
            currentHp = maxHp;
            hpSlider.value = 1f;
            GameClient.Instance.getGoldInformation(100, true);
        }
    }
} 