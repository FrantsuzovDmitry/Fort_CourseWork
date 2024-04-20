using Assets.Scripts;
using Assets.Scripts.GameEntities;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.MediatorModules;
using System;
using System.Collections.Generic;
using static Assets.Scripts.Managers.GameState;

public class Mediator
{
    public bool IsCreatingGroupInProgress => CurrentGameStage == GameStage.PlayerIsCreatingGroupToAttackAFort;
    public bool IsSelectingCardToGiveInProgress => CurrentGameStage == GameStage.CardExchanging;

    private GameStage CurrentGameStage => _gameState.CurrentGameStage;

    private CardExchangeManager _cardExchangeController;
    private UIManager UIManager => UIManager.instance;
    private FortressManager _fortressManager;
    private CardVisualizationManager CardVisualizationManager => CardVisualizationManager.instance;
    private TurnManager TurnManager => TurnManager.instance;
    private PlayerManager PlayerManager => PlayerManager.instance;
    private WinnerDefinitionManager _winneDefinitionrManager;
    private CardManager _cardManager;
    private GameState _gameState;
    private UserActionsValidator _userActionsValidator;
    private CurrentUserIntentionState _currentUserIntentionState;

    private MainDeck _mainDeck;

    private byte CurrentPlayerTurn => TurnManager.CurrentPlayerTurn;

    public void InitializeComponents()
    {
        _mainDeck = new MainDeck();
        _cardManager = new CardManager(_mainDeck);
        _fortressManager = new FortressManager(this);
        _gameState = new GameState();
        _cardExchangeController = new CardExchangeManager(this, _cardManager, _gameState);
        _winneDefinitionrManager = new WinnerDefinitionManager();
        _currentUserIntentionState = new CurrentUserIntentionState();
        _userActionsValidator = new UserActionsValidator();

        UIManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);
    }

    public void OnCardTaken()
    {
        OnAttackStopped();
        var card = _cardManager.GetCardFromDeck();
        CardVisualizationManager.
            CreateCardInCorrectArea(card, CurrentPlayerTurn);
        UIManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        card.InvokeOnCardAppearsEvent();

        if (card.IsCardOnTheTable() && CurrentGameStage != GameStage.GameFinished)
            // Make move again
            this.OnCardTaken();
    }

    public void OnAttackStarted(Fortress fort)
    {
        _currentUserIntentionState.RememberUserFortSelection(fort);
        UIManager.ShowButton(UIButtons.StartAttack);
    }

    public void OnAttackStopped()
    {
        _gameState.OnAttackStopped();
        _currentUserIntentionState.OnAttackStopped();
        UIManager.HideAllButtons();
        CardVisualizationManager.DeselectAllCards();
    }

    public void OnCreatingAttackersGroupStarted()
    {
        _gameState.OnCreatingAttackersGroupStarted();
        UIManager.ShowButton(UIButtons.AttackConfirmation);
    }

    public void OnFortressTriedAttacked()
    {
        bool userActionIsCorrect = _userActionsValidator.ValidateUserAction(
                                            _currentUserIntentionState.GetAttackersGroup(),
                                            _currentUserIntentionState.SelectedFortToAttack);
        if (!userActionIsCorrect)
        {
            UIManager.ShowWarningMessage(_userActionsValidator.LastErrorMessage);
            return;
        }

        CardVisualizationManager.DeselectAllCards();

        _fortressManager.ProcessAttackToFortress(_currentUserIntentionState.SelectedFortToAttack,
                                                    _currentUserIntentionState.GetAttackersGroup(),
                                                            CurrentPlayerTurn);

        UIManager.HideAllButtons();
    }

    public void OnFortressCaptured(Fortress fort)
    {
        var attackerID = CurrentPlayerTurn;
        CardVisualizationManager.MoveCardToPlayer(fort, attackerID);
        CardVisualizationManager.RemoveAttackersFromHand(_currentUserIntentionState.GetAttackersGroup().ToList());
        CardVisualizationManager.DeselectAllCards();

        if (fort.DefendersGroup != null)
        {
            var defenderID = _fortressManager.GetFortressOwner(fort.Rate);
            UIManager.ShowHint("Your fortress has been captured!\n" +
                                "Give to attacker 1 character from defenders group");
            _cardExchangeController.StartCardExchangeProcess(
                    defenderID, attackerID, fort.DefendersGroup.ToList());
        }
    }

    public void OnFortressUnsuccessfulAttacked(byte fortRate)
    {
        var attackerID = CurrentPlayerTurn;
        var defenderID = _fortressManager.GetFortressOwner(fortRate);

        _cardExchangeController.StartCardExchangeProcess(attackerID, defenderID, _cardManager.GetUserHandCharacters(attackerID));
    }

    public void OnFortressAppears(Fortress fort)
    {
        _fortressManager.AddNewFort(fort);
    }

    public void OnFortressDestroyed(byte fortRate)
    {
        _fortressManager.RemoveFortress(fortRate);
    }

    public void OnCardToGiveChosen(Character card)
    {
        _currentUserIntentionState.RememberUserCharacterSelection(card);
        CardVisualizationManager.DeselectAllCards();
        CardVisualizationManager.SetCardRedEmission(card);
        UIManager.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
    }

    public void OnCardGiven()
    {
        var selectedCharacter = _currentUserIntentionState.SelectedCharacter;
        _cardExchangeController.ChangeCardOwner(selectedCharacter);
    }

    public void OnTurnStarted()
    {
        OnAttackStopped();
        _gameState.OnTurnStarted();
        CardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
        UIManager.UpdateCurrentPlayerTurnLabel(CurrentPlayerTurn);
    }

    public void OnTurnEnded()
    {
        UIManager.HideAllButtons();
        TurnManager.AssignNextPlayerTurn();
        this.OnTurnStarted();
    }

    public void OnGameStopped()
    {
        _winneDefinitionrManager.DefineWinner(_fortressManager.FortressOwnerPairs);

        var winnerID = _winneDefinitionrManager.LastWinnerID;
        if (winnerID != Constants.NOT_A_PLAYER_ID)
            UIManager.instance.ShowWinnerPanel(winnerID);
        else
            UIManager.instance.ShowDrawPanel();
        PlayerManager.IncreaseWinNumber(winnerID);
    }

    public void OnSandglassAppears()
    {
        _gameState.OnSandglassAppears();
        _gameState.CheckOfStopGameCondition();
        if (CurrentGameStage == GameStage.GameFinished)
            this.OnGameStopped();
    }

    public void OnRuleAppears(Rule card)
    {
        throw new NotImplementedException();
    }

    public void StartNewRound(byte firstPlayerID)
    {
        List<Card> cardsToRemove = GetCardsToRemove();

        _cardManager.GenerateNewDeck(cardsToRemove);
        TurnManager.AssignTurn(firstPlayerID);
        this.OnTurnStarted();
    }

    /// <summary>
    /// Returns the characters and special cards that winner used.
    /// </summary>
    private List<Card> GetCardsToRemove()
    {
        List<Fortress> winnersForts = _fortressManager.GetPlayersForts(_winneDefinitionrManager.LastWinnerID);
        List<Card> cardsToRemove = new();
        foreach (var fort in winnersForts)
        {
            cardsToRemove.AddRange(fort.DefendersGroup.ToList());
        }
        return cardsToRemove;
    }

    public void StartFirstRound(byte firstPlayerID)
    {
        TurnManager.AssignTurn(firstPlayerID);
        this.OnTurnStarted();
    }

    public void RemoveCharacterFromGroup(Character character) => _currentUserIntentionState.RemoveCharacterFromGroup(character);

    public void AddCharacterToGroup(Character character) => _currentUserIntentionState.AddCharacterToGroup(character);
}