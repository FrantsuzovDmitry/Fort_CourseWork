using Assets.Scripts;
using Assets.Scripts.GameEntities;
using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Threading;
using static Assets.Scripts.Managers.GameState;

public class Mediator
{
	private CardExchangeManager _cardExchangeController;
	private UIManager UIManager => UIManager.instance;
	private FortressManager  _fortressManager;
	private CardVisualizationManager CardVisualizationManager => CardVisualizationManager.instance;
	private TurnManager TurnManager => TurnManager.instance;
	private PlayerManager PlayerManager => PlayerManager.instance;
	private WinnerDefinitionManager _winneDefinitionrManager;
	private CardManager _cardManager;
	private GameState _gameState;

	private MainDeck _mainDeck;

	private byte CurrentPlayerTurn => TurnManager.CurrentPlayerTurn;
	private GameStage CurrentGameStage => _gameState.CurrentGameStage;

    public void InitializeComponents()
	{
		_mainDeck = new MainDeck();
		_cardManager = new CardManager(_mainDeck);
		_fortressManager = new FortressManager(this);
		_gameState = new GameState();
		_cardExchangeController = new CardExchangeManager(this, _cardManager);
		_winneDefinitionrManager = new WinnerDefinitionManager();

		UIManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);
	}

	public void OnCardTaken()
	{
		var card = _cardManager.GetCardFromDeck();
        CardVisualizationManager.
			CreateCardInCorrectArea(card, CurrentPlayerTurn);
		UIManager.UpdateCardNumberText(_cardManager.NumberOfCardsInDeck);

        card.InvokeOnCardAppearsEvent();

        if (card.IsCardOnTheTable() && CurrentGameStage != GameStage.GameFinished)
			// Make move again
			this.OnCardTaken();
	}

	public void OnAttackStarted(byte rate)
    {
        CurrentUserIntentionState.RememberUserFortSelection(rate);
        UIManager.ShowButton(UIButtons.StartAttack);
    }

    public void OnAttackStopped() 
    {
        CurrentUserIntentionState.StopCreatingOfGroupOfCharacters();
        UIManager.HideAllButtons();
        CardVisualizationManager.DeselectAllCards();
    }

	public void OnCreatingAttackersGroupStarted()
	{
		CurrentUserIntentionState.StartCreatingOfGroupOfCharacters();
		UIManager.ShowButton(UIButtons.AttackConfirmation);
	}

    public void OnFortressTriedAttacked()
    {
		CardVisualizationManager.DeselectAllCards();

        if (!CurrentUserIntentionState.CanAttackFortress)
        {
            UIManager.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        _fortressManager.ProcessAttackToFortress(CurrentUserIntentionState.SelectedFortIDToAttack, 
													CurrentUserIntentionState.GetAttackersGroup(),
															CurrentPlayerTurn);

		UIManager.HideAllButtons();
    }

    public void OnFortressCaptured(Fortress fort)
    {
		var attackerID = CurrentPlayerTurn;
		CardVisualizationManager.MoveCardToPlayer(fort, attackerID);
		CardVisualizationManager.RemoveAttackersFromHand(CurrentUserIntentionState.GetAttackersGroup().ToList());
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
        UIManager.ShowHint("Your attack is unsuccessful!\n" +
                                 "Give to defender 1 character from hand");
        _cardExchangeController.StartCardExchangeProcess(attackerID, defenderID, _cardManager.GetUserHandCharacters(attackerID));
    }

    public void OnFortressAppears(Fortress fort)
	{
		_fortressManager.AddNewFort(fort);
	}

	public void OnFortressDestroyed(Fortress fort)
	{
		_fortressManager.RemoveFortress(fort);
	}

    public void OnCardToGiveChosen(Character card)
    {
        CurrentUserIntentionState.RememberUserCharacterSelection(card);
        CardVisualizationManager.DeselectAllCards();
        CardVisualizationManager.SetCardRedEmission(card);
        UIManager.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
    }

    public void OnCardGiven()
    {
		_cardExchangeController.ChangeCardOwnerAndReturnCardsToHand();
    }

	public void OnTurnStarted()
	{
		_gameState.CurrentGameStage = GameStage.PlayerTurn;
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
        _gameState.IncreaseNumberOfSandglasses();
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
}