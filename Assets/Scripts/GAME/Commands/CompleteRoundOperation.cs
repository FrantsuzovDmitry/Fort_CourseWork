using Assets.Scripts.AI;
using System.Collections.Generic;

namespace Assets.Scripts.Managers.Commands
{
    public class CompleteRoundOperation : Command
    {
        private readonly PlayerManager _playerManager;
        private readonly GameInterface _gameInterface;

        public CompleteRoundOperation(PlayerManager playerManager, GameInterface gameInterface)
        {
            _playerManager = playerManager;
            _gameInterface = gameInterface;
        }

        public override void Execute()
        {
            CompleteTheRound();
        }

        private void CompleteTheRound()
        {
            _winnerDefinitionManager.DefineWinner(_fortressManager.FortressOwnerPairs);

            var winnerID = _winnerDefinitionManager.CurrentWinnerID;
            if (winnerID != Constants.NOT_A_PLAYER_ID)
                _uiManager.ShowWinnerPanel(winnerID);
            else
                _uiManager.ShowDrawPanel();
            _playerManager.IncreaseWinsCounter(winnerID);

            NotifyAIAboutEndRound();
        }

        private void NotifyAIAboutEndRound()
        {
            var AIPlayers = new List<AIPlayer>(4);
            for (byte ID = 0; ID < _playerManager.LAST_PLAYER_ID; ID++)
            {
                var currentPlayer = _playerManager.GetPlayer(ID);
                if (currentPlayer is AIPlayer)
                    AIPlayers.Add(currentPlayer as AIPlayer);
            }
            _gameInterface.OnRoundEnded(AIPlayers);
        }
    }
}
