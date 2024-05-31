using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager
{
    public byte LAST_PLAYER_ID => players.Last().ID;

    private readonly List<Player> players;

    public PlayerManager(byte numOfPlayers, byte numOfAI = 0)
    {
        if (numOfPlayers + numOfAI <= 0 || numOfPlayers + numOfAI > 4)
            throw new ArgumentException("Incorrect number of players"); 

        players = new List<Player>(numOfPlayers + numOfAI);

        for (byte i = 0; i < numOfPlayers; i++)
            players.Add(new Player(i));
        for (byte i = numOfPlayers; i < numOfAI; i++)
            players.Add(new AIPlayer(i));

        for (byte i = 0; i < players.Count; i++) players[i].ID = i;
    }

    public void IncreaseWinsCounter(int playerID)
    {
        var plr = players.FirstOrDefault(p => p.ID == playerID);
        if (plr != null) plr.WinsCount++;
    }

    public Player GetPlayer(byte playerID) => players[playerID];
}
