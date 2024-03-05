using Assets.Scripts;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Scripts.Constants;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public byte CurrentPlayerTurn {  get; private set; }

    private void Awake()
    {
        instance = this;
        CurrentPlayerTurn = 0;
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

    public void AssignTurn(byte playerID)
    {
        CurrentPlayerTurn = playerID;
    }
}