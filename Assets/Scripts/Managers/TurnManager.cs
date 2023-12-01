using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;

    public bool isProcessOfCreatingGroup;

    public Action onAttackStopped;

    private void Awake()
    {
        instance = this;
        isProcessOfCreatingGroup = false;
    }

    private void Start()
    {
        StartTurnGameplay(0);
    }

    public void StartTurnGameplay(int playerID)
    {
        currentPlayerTurn = playerID;
        StartTurn();
    }

    public void StartTurn()
    {
        GameplayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
        PlayerManager.instance.AssignTurn(currentPlayerTurn);
        CardManager.instance.ShowMyCards();
        CardManager.instance.HideOpponentsCards();
    }

    public void EndTurn()
    {
        var lastPlayerID = PlayerManager.instance.players.Last().ID;
        if (currentPlayerTurn == lastPlayerID)
        {
            // Firt player turn
            currentPlayerTurn = 0;
        }
        else
        {
            // Next player turn;
            currentPlayerTurn++;
        }
        StartTurn();
    }

    public void CreatingOfGroupOfCharacters()
    {
        Debug.Log("Start creating");
        isProcessOfCreatingGroup = true;
    }

    public void StopCreatingOfGroup()
    {
        Debug.Log("Stop creating");
        isProcessOfCreatingGroup = false;
    }

    private void OnEnable()
    {
        onAttackStopped += StopCreatingOfGroup;
    }

    private void OnDisable()
    {
        onAttackStopped -= StopCreatingOfGroup;
    }
}
