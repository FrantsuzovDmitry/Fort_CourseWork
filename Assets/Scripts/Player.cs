[System.Serializable]
public class Player
{
    public byte ID;
    public bool myTurn;

    public Player(byte ID)
    {
        if (ID < 0 || ID > 4)
            throw new System.Exception("Incorrect  player ID");
        this.ID = ID;
    }
}
