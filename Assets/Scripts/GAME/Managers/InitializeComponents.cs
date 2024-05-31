using Assets.Scripts.AI;
using Assets.Scripts.Managers.Commands;
using Assets.Scripts.Managers.MediatorModules;
using UnityEditor;

namespace Assets.Scripts.Managers
{
    public static class InitializeComponents
    {
        public static Mediator Mediator {  get; private set; }

        private const byte PlayersNumber = 4;

        public static void ExecuteInitialization()
        {
            var cardManager = new CardManager();
            var fortressManager = new FortressManager();
            var gameState = new GameState();
            var winneDefinitionrManager = new WinnerDefinitionManager();
            var currentUserIntentionState = new CurrentUserIntentionState();
            var userActionsValidator = new UserActionsValidator();
            var cardExchangeController = new CardExchangeController(cardManager, gameState);
            var playerManager = new PlayerManager(PlayersNumber);
            var cardVisualizationManager = CardVisualizationManager.instance;
            var uiManager = UIManager.instance;
            var turnManager = new TurnManager(PlayersNumber);
            var gameInterfaceForAI = new GameInterface();

            Mediator = new Mediator(
                uiManager: uiManager,
                fortressManager: fortressManager,
                cardVisualizationManager: cardVisualizationManager,
                turnManager: turnManager,
                playerManager: playerManager,
                winneDefinitionrManager: winneDefinitionrManager,
                cardManager: cardManager,
                gameState: gameState,
                userActionsValidator: userActionsValidator,
                currentUserIntentionState: currentUserIntentionState,
                cardExchangeController: cardExchangeController,
                gameInterface: gameInterfaceForAI
                );


            Command.InitializeComponents(fortressManager, uiManager, cardVisualizationManager, cardManager, winneDefinitionrManager);
            Card.Mediator = Mediator;
            fortressManager.Init(Mediator);
            UIManager.instance.Init(Mediator);
            CardController.SetManagers(turnManager);

            uiManager.UpdateCardNumberText(cardManager.NumberOfCardsInDeck);

        }
    }
}
