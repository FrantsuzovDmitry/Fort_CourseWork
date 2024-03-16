namespace Assets.Scripts.Managers
{
    public class GameState
    {
        public enum GameStage : byte
        {
            PlayerTurn,
            PlayerSelectingCardToGive,
            GameFinished
        }

        public GameStage CurrentGameStage { get; set; }
        public byte NumberOfSandglasses { get; private set; }

        public GameState()
        {
            NumberOfSandglasses = 0;
            CurrentGameStage = GameStage.PlayerTurn;
        }

        public void IncreaseNumberOfSandglasses()
        {
            NumberOfSandglasses++;
        }

        public void CheckOfStopGameCondition()
        {
            if (NumberOfSandglasses == 3)
            {
                CurrentGameStage = GameStage.GameFinished;
            }
        }
    }
}
