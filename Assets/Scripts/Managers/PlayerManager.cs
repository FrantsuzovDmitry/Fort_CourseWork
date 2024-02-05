using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> players = new List<Player>();

    private void Awake()
    {
        instance = this;

        for (byte i = 0; i < players.Count; i++) players[i].ID = i;
    }

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in players)
        {
            player.myTurn = player.ID == currentPlayerTurn;
        }
    }

    public Player FindPlayerByID(int id)
    {
        foreach (Player player in players)
        {
            if (player.ID == id)
            {
                return player;
            }
        }

        // if not found in List
        return null;
    }

    public Player DefineWinner()
    {
        // TODO: Implement the function of define winner later
        return players[0];
    }
}
