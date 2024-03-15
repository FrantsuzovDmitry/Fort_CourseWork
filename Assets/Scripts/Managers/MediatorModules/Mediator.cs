using Assets.Scripts;
using Assets.Scripts.GameEntities;
using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Threading;
using static Assets.Scripts.Managers.GameStateManager;

public class Mediator
{
	private static CardExchangeManager CardExchangeController;
	private static UIManager UIManager => UIManager.instance;
	private static FortressManager FortressManager => FortressManager.instance;
	private static CardVisualizationManager CardVisualizationManager => CardVisualizationManager.instance;
	private static TurnManager TurnManager => TurnManager.instance;
	private static PlayerManager PlayerManager => PlayerManager.instance;
	private static DefinitionWinnerManager DefinitionWinnerManager;

	private static byte CurrentPlayerTurn => TurnManager.CurrentPlayerTurn;

	public static void InitializeComponents()
	{
		MainDeck.Init();
		CardManager.Init();
		GameStateManager.Init();
		UIManager.UpdateCardNumberText(CardManager.NumberOfCardsInDeck);
		CardExchangeController = new CardExchangeManager();
		DefinitionWinnerManager = new DefinitionWinnerManager();
	}

	public static void OnCardTaken()
	{
		var card = CardManager.GetCardFromDeck();
        CardVisualizationManager.
			CreateCardInCorrectArea(card, CurrentPlayerTurn);
		UIManager.UpdateCardNumberText(CardManager.NumberOfCardsInDeck);

        card.InvokeOnCardAppearsEvent();

        if (card.IsCardOnTheTable() && CurrentGameStage != GameStage.GameFinished)
			// Make move again
			Mediator.OnCardTaken();
	}

	public static void OnAttackStarted(byte rate)
    {
        CurrentUserIntentionState.RememberUserFortSelection(rate);
        UIManager.ShowButton(UIButtons.StartAttack);
    }

    public static void OnAttackStopped() 
    {
        CurrentUserIntentionState.StopCreatingOfGroupOfCharacters();
        UIManager.HideAllButtons();
        CardVisualizationManager.DeselectAllCards();
    }

	public static void OnCreatingAttackersGroupStarted()
	{
		CurrentUserIntentionState.StartCreatingOfGroupOfCharacters();
		UIManager.ShowButton(UIButtons.AttackConfirmation);
	}

    public static void OnFortressTriedAttacked()
    {
		CardVisualizationManager.DeselectAllCards();

        if (!CurrentUserIntentionState.CanAttackFortress)
        {
            UIManager.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        FortressManager.ProcessAttackToFortress(CurrentUserIntentionState.SelectedFortIDToAttack, 
													CurrentUserIntentionState.GetAttackersGroup(),
															CurrentPlayerTurn);

		UIManager.HideAllButtons();
    }

    public static void OnFortressCaptured(Fortress fort)
    {
		var attackerID = CurrentPlayerTurn;
		CardVisualizationManager.MoveCardToPlayer(fort, attackerID);
		CardVisualizationManager.RemoveAttackersFromHand(CurrentUserIntentionState.GetAttackersGroup().ToList());
        CardVisualizationManager.DeselectAllCards();

		if (fort.DefendersGroup != null)
		{
			var defenderID = FortressManager.GetFortressOwner(fort.Rate);
            UIManager.ShowHint("Your fortress has been captured!\n" +
								"Give to attacker 1 character from defenders group");
            CardExchangeController.StartCardExchangeProcess(
					defenderID, attackerID, fort.DefendersGroup.ToList());
        }
	}

    public static void OnFortressUnsuccessfulAttacked(byte fortRate)
    {
        var attackerID = CurrentPlayerTurn;
        var defenderID = FortressManager.GetFortressOwner(fortRate);
        UIManager.ShowHint("Your attack is unsuccessful!\n" +
                                 "Give to defender 1 character from hand");
        CardExchangeController.StartCardExchangeProcess(attackerID, defenderID, CardManager.GetUserHandCharacters(attackerID));
    }

    public static void OnFortressAppears(Fortress fort)
	{
		FortressManager.AddNewFort(fort);
	}

	public static void OnFortressDestroyed(Fortress fort)
	{
		FortressManager.RemoveFortress(fort);
	}

    public static void OnCardToGiveChosen(Character card)
    {
        CurrentUserIntentionState.RememberUserCharacterSelection(card);
        CardVisualizationManager.DeselectAllCards();
        CardVisualizationManager.SetCardRedEmission(card);
        UIManager.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
    }

    public static void OnCardGiven()
    {
		CardExchangeController.ChangeCardOwnerAndReturnCardsToHand();
    }

	public static void OnTurnStarted()
	{
		GameStateManager.CurrentGameStage = GameStage.PlayerTurn;
		CardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
		UIManager.UpdateCurrentPlayerTurnLabel(CurrentPlayerTurn);
	}

	public static void OnTurnEnded()
	{
		UIManager.HideAllButtons();
		TurnManager.AssignNextPlayerTurn();
		Mediator.OnTurnStarted();
	}

	public static void OnGameStopped()
	{
		var winnerID = DefinitionWinnerManager.GetWinnerID();
        if (winnerID != Constants.NOT_A_PLAYER_ID)
            UIManager.instance.ShowWinnerPanel(winnerID);
        else
            UIManager.instance.ShowDrawPanel();
        PlayerManager.instance.IncreaseWinNumber(winnerID);
	}

	public static void OnSandglassAppears()
	{
		GameStateManager.IncreaseNumberOfSandglasses();
		GameStateManager.CheckOfStopGameCondition();
	}

	public static void OnRuleAppears(Rule card)
	{
		throw new NotImplementedException();
	}

	public static void StartNewRound(byte firstPlayerID)
	{
		List<Card> cardsToRemove = GetCardsToRemove();

		CardManager.GenerateNewDeck(cardsToRemove);
		TurnManager.AssignTurn(firstPlayerID);
		Mediator.OnTurnStarted();
	}

	/// <summary>
	/// Returns the characters and special cards that winner used.
	/// </summary>
	private static List<Card> GetCardsToRemove()
	{
        List<Fortress> winnersForts = FortressManager.GetPlayersForts(DefinitionWinnerManager.LastWinnerID);
        List<Card> cardsToRemove = new();
        foreach (var fort in winnersForts)
        {
            cardsToRemove.AddRange(fort.DefendersGroup.ToList());
        }
		return cardsToRemove;
    }

    public static void StartFirstRound(byte firstPlayerID)
    {
        TurnManager.AssignTurn(firstPlayerID);
        Mediator.OnTurnStarted();
    }
}