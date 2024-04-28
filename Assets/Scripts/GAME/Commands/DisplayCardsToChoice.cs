using Assets.Scripts.Managers.Commands;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class DisplayCardsToChoice : Command
    {
        private readonly List<Character> _charactersToChoice;

        public DisplayCardsToChoice(List<Character> charactersToChoice)
        {
            _charactersToChoice = charactersToChoice;
        }

        public override void Execute() => DisplayCardsToSelecting();

        private void DisplayCardsToSelecting()
        {
            _uiManager.ToggleCardSelectionPanelVisibility(UIElementState.ON);
            _cardVisualizationManager.DisplayCardsToChoice(_charactersToChoice);
        }
    }
}
