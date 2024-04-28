using System.Collections.Generic;
using System.Linq;

public class PlayerManager
{
    public byte LAST_PLAYER_ID => players.Last().ID;

    private readonly List<Player> players;

    public PlayerManager(byte numOfPlayers)
    {
        players = new List<Player>(numOfPlayers);

        for (byte i = 0; i < players.Count; i++) players[i].ID = i;
    }

    public void IncreaseWinsCounter(int playerID)
    {
        var plr = players.FirstOrDefault(p => p.ID == playerID);
        if (plr != null) plr.WinsCount++;
    }
}
