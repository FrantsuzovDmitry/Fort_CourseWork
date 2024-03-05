using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Player> players = new List<Player>();

    public static PlayerManager instance;

    public byte LAST_PLAYER_ID => players.Last().ID;

    private void Awake()
    {
        instance = this;

        for (byte i = 0; i < players.Count; i++) players[i].ID = i;
    }

    public void IncreaseWinNumber(int playerID)
    {
        var plr = players.FirstOrDefault(p => p.ID == playerID);
        if (plr != null) plr.WinsCount++;
    }
}
