using Game.Network;
using Game.Packets.Request;
using TMPro; // TMP를 사용하기 위해 필요합니다.
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour {
    public Slider hpSlider;
    public TextMeshProUGUI goldText; // GoldText를 드래그해서 넣을 칸

    public float maxHp = 100f;
    private float currentHp;
    private long gold = 0; // 골드 변수

    public static MonsterController Instance {
        get; private set;
    }

    void Start() {
        Instance = this;
        currentHp = maxHp;
        hpSlider.value = 1f;
        UpdateGoldUI(); // 시작하자마자 골드 텍스트 표시
    }

    public void OnMonsterClicked() {
        currentHp -= 50f;
        hpSlider.value = currentHp / maxHp;

        if (currentHp <= 0) {
            currentHp = maxHp;
            hpSlider.value = 1f;
            NettyManager.Instance.Send(LoginPacket.getPong());
        }
    }

    void UpdateGoldUI() {
        goldText.text = "Gold: " + gold.ToString();
    }

    public void AddGold(long amount) {
        gold = amount;
        UpdateGoldUI();
        NettyManager.Instance.Send(LoginPacket.getTest());
    }
}