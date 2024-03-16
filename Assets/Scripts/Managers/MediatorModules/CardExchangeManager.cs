using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public class CardExchangeManager
    {
        private static byte _playerWhichSelectingCardID;
        private static byte _playerWhichGettingCardID;
        private static List<Character> _charactersToChoice;
        private static Character _selectedCharacterToGive;

        private CardManager _cardManager;
        private Mediator _mediator;

        public CardExchangeManager(Mediator mediator, CardManager cardManager)
        {
            _mediator = mediator;
            _cardManager = cardManager;
        }

        public void StartCardExchangeProcess(byte playerWhichSelectingCardID,
                                                    byte playerWhichGettingCardID,
                                                    List<Character> charactersToChoice)
        {
            _playerWhichSelectingCardID = playerWhichSelectingCardID;
            _playerWhichGettingCardID = playerWhichGettingCardID;
            _charactersToChoice = charactersToChoice;

            UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.ON);

            CardVisualizationManager.instance.DisplayCardToChoice(charactersToChoice);
            CurrentUserIntentionState.IsSelectingCardToGiveInProgress = true;
        }

        public void ChangeCardOwnerAndReturnCardsToHand()
        {
            CurrentUserIntentionState.IsSelectingCardToGiveInProgress = false;

            #region CARD EXCHANGING
            _cardManager.ChangeCardOwner(CurrentUserIntentionState.SelectedCharacter, _playerWhichGettingCardID);
            CardVisualizationManager.instance.DeselectAllCards();

            CardVisualizationManager.instance.
                MoveCardToPlayer(CurrentUserIntentionState.SelectedCharacter, _playerWhichGettingCardID);

            ReturnOtherCardsToHand();
            #endregion

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
