namespace Assets.Scripts.Managers.Commands
{
    public class ExchangeCard : Command
    {
        private readonly byte _playerWhichGettingCardID;
        private readonly Character _selectedCharacter;

        public ExchangeCard(byte playerWhichGettingCardID, Character selectedCharacter)
        {
            _playerWhichGettingCardID = playerWhichGettingCardID;
            _selectedCharacter=selectedCharacter;
        }

        public override void Execute()
        {
            ChangeCardOwner();
        }

        private void ChangeCardOwner()
        {
            _cardManager.ChangeCardOwner(_selectedCharacter, _playerWhichGettingCardID);
            _cardVisualizationManager.DeselectAllCards();
            _cardVisualizationManager.MoveCardToPlayer(_selectedCharacter, _playerWhichGettingCardID);

            //TODO: refactor
            _uiManager.ToggleCardSelectionPanelVisibility(UIElementState.OFF);
        }
    }
}
