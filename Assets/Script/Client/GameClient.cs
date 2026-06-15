using Game.Network;
using Game.Network.IO;
using Game.Packets.Request;
using UnityEngine;

public class GameClient : MonoBehaviour {

    private long gold;
    public static GameClient Instance {
        get; private set;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void loadFromCharacter(LittleEndianReader reader) {
        this.gold = reader.ReadLong();
        Debug.Log("현재 골드 수치 : " + this.gold);
        GoldManagerHandler.Instance.UpdateUI(gold);
    }


    public void getGoldInformation(long amount, bool addgold) {
        if (addgold) {
            this.gold += amount;
        } else {
            this.gold = amount;
        }
        GoldManagerHandler.Instance.UpdateUI(gold);
    }
}