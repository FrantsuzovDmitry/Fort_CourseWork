using static Assets.Scripts.Constants;

public class TurnManager
{
    public byte CurrentPlayerTurn { get; private set; }
    public byte FirstTurnPlayer {  get; private set; }  // Player that make first move in current round

    private byte lastPlayerID;

    public TurnManager(byte numberOfPlayers)
    {
        CurrentPlayerTurn = 0;
        lastPlayerID = (byte)(numberOfPlayers - 1);
    }

    public void AssignNextPlayerTurn()
    {
        if (CurrentPlayerTurn == lastPlayerID)
        {
            CurrentPlayerTurn = MIN_PLAYER_ID;
        }
        else
        {
            // Next player turn;
            ++CurrentPlayerTurn;
        }
    }

    public void AssignTurnToFirstPlayer(byte playerID)
    {
        CurrentPlayerTurn = playerID;
    }
}