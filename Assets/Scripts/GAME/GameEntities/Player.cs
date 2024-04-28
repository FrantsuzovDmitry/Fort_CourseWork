using Assets.Scripts;

[System.Serializable]
public class Player
{
    public byte ID;
    public byte WinsCount;

    public Player(byte ID)
    {
        if (ID < Constants.MIN_PLAYER_ID || ID > Constants.MAX_PLAYER_ID)
            throw new System.Exception("Incorrect  player ID");
        this.ID = ID;
    }
}
