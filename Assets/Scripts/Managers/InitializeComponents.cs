using Assets.Scripts.Managers.Commands;
using Assets.Scripts.Managers.MediatorModules;

namespace Assets.Scripts.Managers
{
    public static class InitializeComponents
    {
        public static Mediator Mediator {  get; private set; }

        public static void ExecuteInitialization()
        {
            var _cardManager = new CardManager();
            var _fortressManager = new FortressManager();
            var _gameState = new GameState();
            var _winneDefinitionrManager = new WinnerDefinitionManager();
            var _currentUserIntentionState = new CurrentUserIntentionState();
            var _userActionsValidator = new UserActionsValidator();
            var _cardExchangeController = new CardExchangeController(_cardManager, _gameState);
            var _playerManager = PlayerManager.instance;
            var _cardVisualizationManager = CardVisualizationManager.instance;
            var _uiManager = UIManager.instance;
            var _turnManager = TurnManager.instance;

            Mediator = new Mediator(
                uiManager: _uiManager,
                fortressManager: _fortressManager,
                cardVisualizationManager: _cardVisualizationManager,
                turnManager: _turnManager,
                playerManager: _playerManager,
                winneDefinitionrManager: _winneDefinitionrManager,
                cardManager: _cardManager,
                gameState: _gameState,
                userActionsValidator: _userActionsValidator,
                currentUserIntentionState: _currentUserIntentionState,
                cardExchangeController: _cardExchangeController
                );

            Command.InitializeComponents(_fortressManager, _uiManager, _cardVisualizationManager, _cardManager);
            Card.Mediator = Mediator;
            _fortressManager.Init(Mediator);
            UIManager.instance.Init(Mediator);

            _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        }
    }
}
