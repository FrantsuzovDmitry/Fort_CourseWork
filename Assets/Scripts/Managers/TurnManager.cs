using Assets.Scripts;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Scripts.Constants;

public class TurnManager : MonoBehaviour
{
    public enum GameStage
    {
        StandartStage,          // Getting card
        SelectingCharacters     // I'll try to generalize 
        //SelectingCharacterFromHand,
        //SelectingCharacterFromDefenders,
    }

    public static TurnManager instance;
    public byte CurrentPlayerTurn {  get; private set; }
    public GameStage CurrentGameStage { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrentPlayerTurn = 0;
        CurrentGameStage = GameStage.StandartStage;

        Mediator.OnTurnStarted();
    }

    public void AssignNextPlayerTurn()
    {
        if (CurrentPlayerTurn == LAST_PLAYER_ID)
        {
            CurrentPlayerTurn = MIN_PLAYER_ID;
        }
        else
        {
            // Next player turn;
            ++CurrentPlayerTurn;
        }
    }

    public void AssignSelectingCard(byte playerID)
    {
        CurrentGameStage = GameStage.SelectingCharacters;
        CurrentPlayerTurn = playerID;
    }
}