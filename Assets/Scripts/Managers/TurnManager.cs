using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;

    public bool NowTheProcessOfCreatingGroupIsUnderway {  get; private set; }

    private const byte FIRST_PLAYER_ID = 0;
    private byte LAST_PLAYER_ID;

    private void Awake()
    {
        instance = this;
        NowTheProcessOfCreatingGroupIsUnderway = false;
    }

    private void Start()
    {
        StartTurnOfPlayer(0);

        LAST_PLAYER_ID = (byte)PlayerManager.instance.players.Last().ID;
    }

    public void StartTurnOfPlayer(int playerID)
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
        if (currentPlayerTurn == LAST_PLAYER_ID)
        {
            currentPlayerTurn = FIRST_PLAYER_ID;
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
        NowTheProcessOfCreatingGroupIsUnderway = true;
    }

    public void StopCreatingOfGroup()
    {
        Debug.Log("Stop creating");
        NowTheProcessOfCreatingGroupIsUnderway = false;
    }

    private void OnEnable()
    {
        Observer.onAttackStopped += StopCreatingOfGroup;
    }

    private void OnDisable()
    {
		Observer.onAttackStopped -= StopCreatingOfGroup;
    }
}
