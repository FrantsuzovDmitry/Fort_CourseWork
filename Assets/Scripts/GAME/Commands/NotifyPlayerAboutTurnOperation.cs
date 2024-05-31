using Assets.Scripts.AI;
using Assets.Scripts.Managers.Commands;

namespace Assets.Scripts.GAME.Commands
{
    public class NotifyPlayerAboutTurnOperation : Command
    {
        private PlayerManager _playerManager;
        private GameInterface _gameInterface;
        private byte currentPlayerTurn;

        public NotifyPlayerAboutTurnOperation(PlayerManager playerManager, GameInterface gameInterface, byte currentPlayerID)
        {
            _playerManager=playerManager;
            _gameInterface = gameInterface;
            currentPlayerTurn = currentPlayerID;
        }

        public override void Execute()
        {
            NotifyPlayer();
        }

        private void NotifyPlayer()
        {
            var currentPlayer = _playerManager.GetPlayer(currentPlayerTurn);
            if (currentPlayer is AIPlayer)
            {
                var aiPlayer = _playerManager.GetPlayer(currentPlayerTurn) as AIPlayer;
                _gameInterface.NotifyAIAboutTurn(aiPlayer);
            }
        }
    }
}
