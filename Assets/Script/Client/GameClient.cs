using Game.Network;
using Game.Network.IO;
using Game.Packets.Request;
using System;
using UnityEngine;

public class GameClient : MonoBehaviour {
    public static GameClient Instance {
        get; private set;
    }
    public event Action<DataCategory, CharacterData> OnDataChanged;

    public CharacterData Data { get; private set; } = new CharacterData();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadFromCharacter(LittleEndianReader reader) {
        Data.gold = reader.ReadLong();
        Data.level = reader.ReadInt();
        if (OnDataChanged == null)
            Debug.LogWarning("[GameClient] 구독자가 0명입니다!");
        else
            Debug.Log($"[GameClient] 구독자 수: {OnDataChanged.GetInvocationList().Length}");
        OnDataChanged?.Invoke(DataCategory.ALL, Data);
    }


    public void getGoldInformation(long amount, bool changegoldstatus) {
        if (changegoldstatus) {
            Data.gold += amount;
        } else {
            Data.gold = amount;
        }
        OnDataChanged?.Invoke(DataCategory.GOLD, Data);
        NettyManager.Instance.Send(LoginPacket.getGoldInformation(Data.gold));
    }
}