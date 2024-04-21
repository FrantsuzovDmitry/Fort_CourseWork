using Assets.Scripts;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Commands;
using Assets.Scripts.Managers.MediatorModules;
using System;
using System.Collections.Generic;
using static Assets.Scripts.Managers.GameState;

/// <summary>
/// Redirects requests between modules
/// </summary>
public class Mediator
{
    public bool IsCreatingGroupInProgress => CurrentGameStage == GameStage.PlayerIsCreatingGroupToAttackAFort;
    public bool IsSelectingCardToGiveInProgress => CurrentGameStage == GameStage.CardExchanging;

    private GameStage CurrentGameStage => _gameState.CurrentGameStage;

    private readonly UIManager _uiManager;
    private readonly FortressManager _fortressManager;
    private readonly CardVisualizationManager _cardVisualizationManager;
    private readonly TurnManager _turnManager;
    private readonly PlayerManager _playerManager;
    private readonly WinnerDefinitionManager _winneDefinitionrManager;
    private readonly CardManager _cardManager;
    private readonly GameState _gameState;
    private readonly UserActionsValidator _userActionsValidator;
    private readonly CurrentUserIntentionState _currentUserIntentionState;
    private readonly CardExchangeController _cardExchangeController;

    public Mediator(UIManager uiManager, FortressManager fortressManager, CardVisualizationManager cardVisualizationManager, TurnManager turnManager, PlayerManager playerManager, WinnerDefinitionManager winneDefinitionrManager, CardManager cardManager, GameState gameState, UserActionsValidator userActionsValidator, CurrentUserIntentionState currentUserIntentionState, CardExchangeController cardExchangeController)
    {
        _uiManager=uiManager;
        _fortressManager=fortressManager;
        _cardVisualizationManager=cardVisualizationManager;
        _turnManager=turnManager;
        _playerManager=playerManager;
        _winneDefinitionrManager=winneDefinitionrManager;
        _cardManager=cardManager;
        _gameState=gameState;
        _userActionsValidator=userActionsValidator;
        _currentUserIntentionState=currentUserIntentionState;
        _cardExchangeController=cardExchangeController;
    }

    private byte CurrentPlayerTurn => _turnManager.CurrentPlayerTurn;

    // legacy
    //public void InitializeComponents()
    //{
    //    _cardManager = new CardManager();
    //    _fortressManager = new FortressManager();
    //    _gameState = new GameState();
    //    _cardExchangeController = new CardExchangeManager(_cardManager, _gameState);
    //    _winneDefinitionrManager = new WinnerDefinitionManager();
    //    _currentUserIntentionState = new CurrentUserIntentionState();
    //    _userActionsValidator = new UserActionsValidator();
    //    _playerManager = PlayerManager.instance;
    //    _cardVisualizationManager = CardVisualizationManager.instance;
    //    _uiManager = UIManager.instance;
    //    _turnManager = TurnManager.instance;

    //    Command.InitializeComponents(_fortressManager, _uiManager, _cardVisualizationManager);
    //    Card.Mediator = this;
    //    _fortressManager.Initialize(this);

    //    _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);
    //}

    public void OnCardTaken()
    {
        OnAttackStopped();
        var card = _cardManager.GetCardFromDeck();
        _cardVisualizationManager.OnCardTaken(card, CurrentPlayerTurn);
        _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        card.InvokeOnCardAppearsEvent();

        if (card.CardShouldBeOnTheTable() && CurrentGameStage != GameStage.GameFinished)
            // Make move again
            this.OnCardTaken();
    }

    public void OnAttackStarted(Fortress fort)
    {
        _currentUserIntentionState.RememberUserFortSelection(fort);
        _uiManager.ShowButton(UIButtons.StartAttack);
    }

    public void OnAttackStopped()
    {
        _gameState.OnAttackStopped();
        _currentUserIntentionState.OnAttackStopped();
        _uiManager.HideAllButtons();
        this.SetStandardState();
    }

    public void OnCreatingAttackersGroupStarted()
    {
        _gameState.OnCreatingAttackersGroupStarted();
        _uiManager.ShowButton(UIButtons.AttackConfirmation);
    }

    public void OnFortressTriedAttacked()
    {
        bool userActionIsCorrect = _userActionsValidator.ValidateUserAction(
                                            _currentUserIntentionState.GetAttackersGroup(),
                                            _currentUserIntentionState.SelectedFortToAttack);
        if (!userActionIsCorrect)
        {
            _uiManager.ShowWarningMessage(_userActionsValidator.LastErrorMessage);
            return;
        }

        this.SetStandardState();

        _fortressManager.ProcessAttackingToFortress(_currentUserIntentionState.SelectedFortToAttack,
                                                    _currentUserIntentionState.GetAttackersGroup(),
                                                            CurrentPlayerTurn);

        _uiManager.HideAllButtons();
    }

    public void OnFortressCaptured(Fortress fort)
    {
        var attackerID = CurrentPlayerTurn;
        _cardVisualizationManager.MoveCardToPlayer(fort, attackerID);
        _cardVisualizationManager.RemoveAttackersFromHand(_currentUserIntentionState.GetAttackersGroup().ToList());
        this.SetStandardState();

        if (fort.DefendersGroup != null)
        {
            var defenderID = _fortressManager.GetFortressOwner(fort.Rate);
            _uiManager.ShowHint("Your fortress has been captured!\n" +
                                "Give to attacker 1 character from defenders group");

            _cardExchangeController.OnFortressCaptured(attackerID, defenderID, fort);
        }
    }

    public void OnFortressUnsuccessfulAttacked(byte fortRate)
    {
        var attackerID = CurrentPlayerTurn;
        var defenderID = _fortressManager.GetFortressOwner(fortRate);

        UIManager.instance.ShowHint(
                "Your attack is unsuccessful!\n " +
                "Give to defender 1 character from hand");

        _cardExchangeController.OnFortressUnsuccessfulAttacked(attackerID: attackerID, defenderID: defenderID);
    }

    public void OnFortressAppears(Fortress fort)
    {
        _fortressManager.AddNewFort(fort);
    }

    public void OnFortressDestroyed(Fortress fort)
    {
        new DestroyFortress(fort, CurrentPlayerTurn)
            .Execute();
        _cardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
    }

    public void OnCardToGiveChosen(Character card)
    {
        _currentUserIntentionState.RememberUserCharacterSelection(card);
        this.SetStandardState();
        _cardVisualizationManager.SetCardRedEmission(card);
        _uiManager.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
    }

    public void OnCardGiven()
    {
        var selectedCharacter = _currentUserIntentionState.SelectedCharacter;
        _cardExchangeController.OnCardGiven(selectedCharacter);

        _cardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
        //this.OnTurnEnded();
    }

    public void OnTurnStarted()
    {
        OnAttackStopped();
        _gameState.OnTurnStarted();
        _cardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
        _uiManager.UpdateCurrentPlayerTurnLabel(CurrentPlayerTurn);
    }

    public void OnTurnEnded()
    {
        _uiManager.HideAllButtons();
        _turnManager.AssignNextPlayerTurn();
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
        _playerManager.IncreaseWinNumber(winnerID);
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
        _turnManager.AssignTurn(firstPlayerID);
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
        _turnManager.AssignTurn(firstPlayerID);
        this.OnTurnStarted();
    }

    public void RemoveCharacterFromGroup(Character character) => _currentUserIntentionState.RemoveCharacterFromGroup(character);

    public void AddCharacterToGroup(Character character) => _currentUserIntentionState.AddCharacterToGroup(character);

    private void ExecuteCommand(Command command) => command.Execute();

    private void SetStandardState()
    {
        _cardVisualizationManager.DeselectAllCards();
    }
}