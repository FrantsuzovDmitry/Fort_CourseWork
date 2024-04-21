using Assets.Scripts.Managers.Commands;

namespace Assets.Scripts.Managers.MediatorModules
{
    public class CardExchangeController
    {
        private byte _playerGettingCardID;
        private byte _playerSelectingCardID;

        private readonly CardManager _cardManager;
        private readonly GameState _gameState;

        public CardExchangeController(CardManager cardManager, GameState gameState)
        {
            _cardManager=cardManager;
            _gameState=gameState;
        }

        public void OnCardGiven(Character selectedCharacter)
        {
            new ExchangeCard(_playerGettingCardID, selectedCharacter)
                .Execute();

            var cardsToReturn = _cardManager.GetUserHandCharacters(_playerSelectingCardID);
            new ReturnUserCardsToHand(_playerSelectingCardID, cardsToReturn)
                .Execute();

            _gameState.OnCardExchangingStopped();
        }

        public void OnFortressUnsuccessfulAttacked(byte attackerID, byte defenderID)
        {
            _playerGettingCardID = defenderID;
            _playerSelectingCardID = attackerID;

            new DisplayCardsToChoice(_cardManager.GetUserHandCharacters(attackerID))
                .Execute();

            _gameState.OnCardExchangingStarted();
        }

        public void OnFortressCaptured(byte attackerID, byte defenderID, Fortress fort)
        {
            _playerGettingCardID = attackerID;
            _playerSelectingCardID = defenderID;

            new DisplayCardsToChoice(fort.DefendersGroup.ToList())
                .Execute();

            _gameState.OnCardExchangingStarted();
        }
    }
}
