namespace Assets.Scripts.Managers.Commands
{
    public abstract class Command
    {
        protected static FortressManager _fortressManager;
        protected static UIManager _uiManager;
        protected static CardVisualizationManager _cardVisualizationManager;
        protected static CardManager _cardManager;
        protected static WinnerDefinitionManager _winnerDefinitionManager;

        public abstract void Execute();

        public static void InitializeComponents(FortressManager fm, 
                                                UIManager uim, 
                                                CardVisualizationManager cvm, 
                                                CardManager cardManager,
                                                WinnerDefinitionManager wdm)
        {
            _fortressManager = fm;
            _uiManager = uim;
            _cardVisualizationManager = cvm;
            _cardManager = cardManager;
            _winnerDefinitionManager = wdm;
        }
    }
}
