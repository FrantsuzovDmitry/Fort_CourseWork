using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public static class GameStateManager
    {
        public enum GameStage : byte
        {
            PlayerTurn,
            PlayerSelectingCardToGive,     
            GameFinished
        }

        public static GameStage CurrentGameStage { get; set; }

        public static byte NumberOfSandglasses { get; private set; }

        public static void Init()
        {
            NumberOfSandglasses = 0;
            CurrentGameStage = GameStage.PlayerTurn;
        }

        public static void IncreaseNumberOfSandglasses()
        {
            NumberOfSandglasses++;
        }

        public static void CheckOfStopGameCondition()
        {
            if (NumberOfSandglasses == 3)
            {
                CurrentGameStage = GameStage.GameFinished;
                Mediator.OnGameStopped();
            }
        }
    }
}
