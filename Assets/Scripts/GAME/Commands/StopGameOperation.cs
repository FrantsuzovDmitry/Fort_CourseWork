namespace Assets.Scripts.Managers.Commands
{
    public class StopGameOperation : Command
    {
        private readonly PlayerManager _playerManager;

        public StopGameOperation(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public override void Execute()
        {
            OnGameStopped();
        }

        private void OnGameStopped()
        {
            _winnerDefinitionManager.DefineWinner(_fortressManager.FortressOwnerPairs);

            var winnerID = _winnerDefinitionManager.CurrentWinnerID;
            if (winnerID != Constants.NOT_A_PLAYER_ID)
                _uiManager.ShowWinnerPanel(winnerID);
            else
                _uiManager.ShowDrawPanel();
            _playerManager.IncreaseWinsCounter(winnerID);
        }
    }
}
