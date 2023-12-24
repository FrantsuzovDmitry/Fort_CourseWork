[System.Serializable]
public class Player
{
    public int ID;
    public bool myTurn;

    public Player(int ID)
    {
        if (ID < 0 || ID > 4)
            throw new System.Exception("Incorrect  player ID");
        this.ID = ID;
    }
}
