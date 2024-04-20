using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class CardExchangeManager
    {
        private static byte _playerWhichSelectingCardID;
        private static byte _playerWhichGettingCardID;

        private CardManager _cardManager;
        private Mediator _mediator;
        private GameState _gameState;

        public CardExchangeManager(Mediator mediator, CardManager cardManager, GameState gameState)
        {
            _mediator = mediator;
            _cardManager = cardManager;
            _gameState = gameState;
        }

        public void StartCardExchangeProcess(byte playerWhichSelectingCardID,
                                                    byte playerWhichGettingCardID,
                                                    List<Character> charactersToChoice)
        {
            _playerWhichSelectingCardID = playerWhichSelectingCardID;
            _playerWhichGettingCardID = playerWhichGettingCardID;

            UIManager.instance.ShowHint(
                "Your attack is unsuccessful!\n " +
                "Give to defender 1 character from hand");
            UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.ON);

            CardVisualizationManager.instance.DisplayCardToChoice(charactersToChoice);
            _gameState.OnCardExchangingStarted();
        }

        public void ChangeCardOwner(Character selectedCharacter)
        {
            _gameState.OnCardExchangingStopped();

            // CARD EXCHANGING
            _cardManager.ChangeCardOwner(selectedCharacter, _playerWhichGettingCardID);
            CardVisualizationManager.instance.DeselectAllCards();
            CardVisualizationManager.instance.
                MoveCardToPlayer(selectedCharacter, _playerWhichGettingCardID);
            ReturnOtherCardsToHand();

            UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.OFF);
            _mediator.OnTurnEnded();
        }

        private void ReturnOtherCardsToHand()
        {
            var otherAttackersCharacters = _cardManager.GetUserHandCharacters(_playerWhichSelectingCardID);
            foreach (var card in otherAttackersCharacters)
                CardVisualizationManager.instance.MoveCardToPlayer(card, _playerWhichSelectingCardID);
        }
    }
}
