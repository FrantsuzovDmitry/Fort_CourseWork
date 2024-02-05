using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public byte currentPlayerTurn;

    private byte LAST_PLAYER_ID;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartTurnOfPlayer(0);

        LAST_PLAYER_ID = (byte)PlayerManager.instance.players.Last().ID;
    }

    public void StartTurnOfPlayer(byte playerID)
    {
        currentPlayerTurn = playerID;
        StartTurn();
    }

    public void StartTurn()
    {
        GameplayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
        PlayerManager.instance.AssignTurn(currentPlayerTurn);
        CardVisualizationManager.instance.ShowCurrentPlayerCards();
        CardVisualizationManager.instance.HideOpponentsCards();
    }

    public void EndTurn()
    {
        if (currentPlayerTurn == LAST_PLAYER_ID)
        {
            currentPlayerTurn = Constants.MIN_PLAYER_ID;
        }
        else
        {
            // Next player turn;
            currentPlayerTurn++;
        }
        StartTurn();
    }

    public void StartCreatingOfGroupOfCharacters()
    {
        Debug.Log("Start creating");
    }

    public void StopCreatingOfGroup()
    {
        Debug.Log("Stop creating");
    }
}