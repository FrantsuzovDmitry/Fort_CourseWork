using System.Collections.Generic;

namespace Assets.Scripts.Managers.Commands
{
    public class StartNewRoundOperation : Command
    {
        private readonly byte currentWinnerID;
        private readonly byte lastWinnerID;
        private readonly GameState _gameState;
        private readonly TurnManager _turnManager;

        public StartNewRoundOperation(byte lastWinnerID, byte currentWinnerID, GameState gameState, TurnManager turnManager)
        {
            this.lastWinnerID=lastWinnerID;
            this.currentWinnerID=currentWinnerID;
            _gameState=gameState;
            _turnManager=turnManager;
        }

        public override void Execute()
        {
            StartNewRound();
        }

        private void StartNewRound()
        {
            _turnManager.AssignTurnToFirstPlayer(GetFirstTurnPlayerID());

            List<Fortress> winnersForts = _fortressManager.GetPlayersForts(currentWinnerID);
            var winnersCardsInHisFortresses = GetWinnersCardsInHisFortresses(winnersForts);
            _cardManager.OnNewRoundStarted(winnersCardsInHisFortresses);

            _cardVisualizationManager.OnNewRoundStarted();
            _fortressManager.OnNewRoundStarted();

            _uiManager.HideWinnerPanel();
            _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

            _gameState.OnNewRoundStarted();
            _winnerDefinitionManager.OnNewRoundStarted();
        }

        private byte GetFirstTurnPlayerID()
        {
            return lastWinnerID;
        }

        private List<Card> GetWinnersCardsInHisFortresses(List<Fortress> winnersForts)
        {
            List<Card> winnersCardsInHisFortresses = new();
            foreach (var fort in winnersForts)
            {
                winnersCardsInHisFortresses.AddRange(fort.DefendersGroup.ToList());
            }
            return winnersCardsInHisFortresses;
        }
    }
}
