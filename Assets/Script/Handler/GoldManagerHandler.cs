using Game.Network;
using Game.Packets.Request;
using TMPro;
using UnityEngine;

public class GoldManagerHandler : MonoBehaviour {
    public static GoldManagerHandler Instance {
        get; private set;
    }

    [SerializeField] private TextMeshProUGUI goldText;

    void Awake() {
        Instance = this;
    }
    
    public void UpdateUI(long currentGold) {
        if (goldText != null) {
            goldText.text = $"Gold: {currentGold}";
            NettyManager.Instance.Send(LoginPacket.getGoldInformation(currentGold));
        }
    }
}