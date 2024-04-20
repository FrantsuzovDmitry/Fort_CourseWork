using System;

namespace Assets.Scripts.Managers
{
    public class GameState
    {
        public enum GameStage : byte
        {
            PlayerTurn,
            PlayerIsCreatingGroupToAttackAFort,
            CardExchanging,
            GameFinished
        }

        public GameStage CurrentGameStage { get; private set; }
        public byte NumberOfSandglasses { get; private set; }

        public GameState()
        {
            NumberOfSandglasses = 0;
            CurrentGameStage = GameStage.PlayerTurn;
        }

        public void OnSandglassAppears()
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

        public void OnTurnStarted() => SetBaseState();

        public void OnCreatingAttackersGroupStarted() => CurrentGameStage = GameStage.PlayerIsCreatingGroupToAttackAFort;

        public void OnAttackStopped() => CurrentGameStage = GameStage.PlayerTurn;

        public void OnCardExchangingStarted() => CurrentGameStage = GameStage.CardExchanging;

        public void OnCardExchangingStopped() => SetBaseState();

        private void SetBaseState() => CurrentGameStage = GameStage.PlayerTurn;
    }
}
