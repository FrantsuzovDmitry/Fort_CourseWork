using Assets.Scripts.Managers.Commands;
using Assets.Scripts.Managers.MediatorModules;

namespace Assets.Scripts.Managers
{
    public static class InitializeComponents
    {
        public static Mediator Mediator {  get; private set; }

        private const byte PlayersNumber = 4;

        public static void ExecuteInitialization()
        {
            var _cardManager = new CardManager();
            var _fortressManager = new FortressManager();
            var _gameState = new GameState();
            var _winneDefinitionrManager = new WinnerDefinitionManager();
            var _currentUserIntentionState = new CurrentUserIntentionState();
            var _userActionsValidator = new UserActionsValidator();
            var _cardExchangeController = new CardExchangeController(_cardManager, _gameState);
            var _playerManager = new PlayerManager(PlayersNumber);
            var _cardVisualizationManager = CardVisualizationManager.instance;
            var _uiManager = UIManager.instance;
            var _turnManager = new TurnManager(PlayersNumber);

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

            Command.InitializeComponents(_fortressManager, _uiManager, _cardVisualizationManager, _cardManager, _winneDefinitionrManager);
            Card.Mediator = Mediator;
            _fortressManager.Init(Mediator);
            UIManager.instance.Init(Mediator);
            CardController.SetManagers(_turnManager);

            _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        }
    }
}
