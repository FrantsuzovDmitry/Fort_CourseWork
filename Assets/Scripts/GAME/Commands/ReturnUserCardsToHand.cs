using System;
using System.Collections.Generic;

namespace Assets.Scripts.Managers.Commands
{
    public class ReturnUserCardsToHand : Command
    {
        private byte _playerWhichReturningCardsID;
        private List<Character> _cardsThatShouldBeReturned;

        public ReturnUserCardsToHand(byte playerWhichReturningCardsID, List<Character> cardsThatShouldBeReturned)
        {
            _playerWhichReturningCardsID = playerWhichReturningCardsID;
            _cardsThatShouldBeReturned = cardsThatShouldBeReturned;
        }

        public override void Execute()
        {
            ReturnCardsToHand();
        }

        private void ReturnCardsToHand()
        {
            foreach (var card in _cardsThatShouldBeReturned)
                _cardVisualizationManager.MoveCardToPlayer(card, _playerWhichReturningCardsID);
        }
    }
}
