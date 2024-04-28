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

    private byte CurrentPlayerTurn => _turnManager.CurrentPlayerTurn;
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

    public void OnCardTaken()
    {
        OnAttackStopped();
        var card = _cardManager.GetCardFromDeck(CurrentPlayerTurn);
        _cardVisualizationManager.OnCardTaken(card, CurrentPlayerTurn);
        _uiManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        card.InvokeOnCardAppearsEvent();

        if (card.CardShouldBeOnTheTable() && CurrentGameStage != GameStage.GameFinished)
            // Make move again
            this.OnCardTaken();
        else
            this.OnTurnEnded();
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
        List<Character> attackersGroup = _currentUserIntentionState.GetAttackersGroup().ToList();

        _cardVisualizationManager.OnFortressCaptured(attackerID, fort, attackersGroup);
        _cardManager.OnFortressCaptured(attackerID, attackersGroup);
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
        new StopGameOperation(_playerManager).Execute();
    }

    public void OnSandglassAppears()
    {
        _gameState.OnSandglassAppears();
        if (_gameState.IsTheGameOver)
            this.OnGameStopped();
    }

    public void OnRuleAppears(Rule card)
    {
        throw new NotImplementedException();
    }

    public void OnNewRoundStarted()
    {
        var lastWinnerID = _winneDefinitionrManager.LastWinnerID;
        var currentWinnerID = _winneDefinitionrManager.CurrentWinnerID;
        new StartNewRoundOperation(lastWinnerID, currentWinnerID, _gameState, _turnManager).Execute();

        this.OnTurnStarted();
    }

    public void StartFirstRound(byte firstPlayerID)
    {
        _turnManager.AssignTurnToFirstPlayer(firstPlayerID);
        this.OnTurnStarted();
    }

    public void RemoveCharacterFromGroup(Character character) => _currentUserIntentionState.RemoveCharacterFromGroup(character);

    public void AddCharacterToGroup(Character character) => _currentUserIntentionState.AddCharacterToGroup(character);

    private void SetStandardState()
    {
        _cardVisualizationManager.DeselectAllCards();
    }
}