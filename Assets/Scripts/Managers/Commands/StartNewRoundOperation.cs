using System.Collections.Generic;
using static Assets.Scripts.Constants;

namespace Assets.Scripts.Managers.Commands
{
    public class StartNewRoundOperation : Command
    {
        private readonly byte lastWinnerID;
        private readonly byte currentWinnerID;

        private TurnManager TurnManager => TurnManager.instance;

        public StartNewRoundOperation(byte lastWinnerID, byte currentWinnerID)
        {
            this.lastWinnerID=lastWinnerID;
            this.currentWinnerID=currentWinnerID;
        }

        public override void Execute()
        {
            StartNewRound();
        }

        private void StartNewRound()
        {
            TurnManager.AssignTurnToFirstPlayer(GetFirstPlayerID());

            List<Fortress> winnersForts = _fortressManager.GetPlayersForts(currentWinnerID);
            var winnersCardsInHisFortresses = GetWinnersCardsInHisFortresses(winnersForts);
            _cardManager.OnNewRoundStarted(winnersCardsInHisFortresses);

            _cardVisualizationManager.OnNewRoundStarted();
            _fortressManager.OnNewRoundStarted();

            _uiManager.HideWinnerPanel();
        }

        byte GetFirstPlayerID()
        {
            if (currentWinnerID == NOT_A_PLAYER_ID)
                return (byte)((lastWinnerID + 1) % 4);
            else
                return currentWinnerID;
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
