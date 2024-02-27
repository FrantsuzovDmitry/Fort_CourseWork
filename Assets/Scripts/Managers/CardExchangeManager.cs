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

        public void StartCardExchangeProcess(byte playerWhichSelectingCardID,
                                                    byte playerWhichGettingCardID,
                                                    List<Character> charactersToChoice)
        {
            _playerWhichSelectingCardID = playerWhichSelectingCardID;
            _playerWhichGettingCardID = playerWhichGettingCardID;
            _charactersToChoice = charactersToChoice;

            UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.ON);

            CardVisualizationManager.instance.DisplayCardToChoice(charactersToChoice);
            UserStateManager.IsSelectingCardToGiveInProgress = true;
        }

        // Is it need?
        public void OnCardChosen(Character card)
        {
            CardVisualizationManager.instance.DeselectAllCards();
            CardVisualizationManager.instance.SetCardRedEmission(card);
            UIManager.instance.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
        }

        public void ChangeCardOwnerAndReturnCardsToHand()
        {
            UserStateManager.IsSelectingCardToGiveInProgress = false;

            #region CARD EXCHANGING
            CardManager.ChangeCardOwner(UserStateManager.SelectedCharacter, _playerWhichGettingCardID);
            CardVisualizationManager.instance.DeselectAllCards();

            CardVisualizationManager.instance.
                MoveCardToPlayer(UserStateManager.SelectedCharacter, _playerWhichGettingCardID);

            ReturnOtherCardsToHand();
            #endregion

            UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.OFF);
            Mediator.OnTurnEnded();
        }

        private void ReturnOtherCardsToHand()
        {
            var otherAttackersCharacters = CardManager.GetUserHandCharacters(_playerWhichSelectingCardID);
            foreach (var card in otherAttackersCharacters)
                CardVisualizationManager.instance.MoveCardToPlayer(card, _playerWhichSelectingCardID);
        }
    }
}
