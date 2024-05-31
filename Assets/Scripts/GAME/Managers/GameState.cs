﻿namespace Assets.Scripts.Managers
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
            CheckEndRoundCondition();
        }

        public bool IsTheGameOver => CurrentGameStage == GameStage.GameFinished;

        public void OnTurnStarted() => SetBaseState();

        public void OnCreatingAttackersGroupStarted() => CurrentGameStage = GameStage.PlayerIsCreatingGroupToAttackAFort;

        public void OnAttackStopped() => CurrentGameStage = GameStage.PlayerTurn;

        public void OnCardExchangingStarted() => CurrentGameStage = GameStage.CardExchanging;

        public void OnCardExchangingStopped() => SetBaseState();

        public void OnNewRoundStarted() => NumberOfSandglasses = 0;

        private void SetBaseState() => CurrentGameStage = GameStage.PlayerTurn;

        private void CheckEndRoundCondition()
        {
            if (NumberOfSandglasses == 3)
                CurrentGameStage = GameStage.GameFinished;
        }
    }
}