using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> players = new List<Player>();

    private void Awake()
    {
        instance = this;
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

    //public void EndGame()
    //{
    //    Player winner = DefineWinner();
    //    // Show panel with results
    //    UIManager.instance.ShowWinnerPanel(winner);
    //}

    public Player DefineWinner()
    {
        // Implement the function of define winner later
        return players[0];
    }
}
