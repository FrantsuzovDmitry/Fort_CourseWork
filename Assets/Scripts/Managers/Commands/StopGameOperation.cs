using System;

namespace Assets.Scripts.Managers.Commands
{
    public class StopGameOperation : Command
    {
        private PlayerManager PlayerManager => PlayerManager.instance;

        public override void Execute()
        {
            StopTheGame();
        }

        private void StopTheGame()
        {
            _winnerDefinitionManager.DefineWinner(_fortressManager.FortressOwnerPairs);

            var winnerID = _winnerDefinitionManager.CurrentWinnerID;
            if (winnerID != Constants.NOT_A_PLAYER_ID)
                UIManager.instance.ShowWinnerPanel(winnerID);
            else
                UIManager.instance.ShowDrawPanel();
            PlayerManager.IncreaseWinsCounter(winnerID);
        }
    }
}
