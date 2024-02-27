using Assets.Scripts;
using Assets.Scripts.Managers;
using System;

public static class Mediator
{
	private static CardExchangeManager CardExchangeController;
	private static UIManager UIManager => UIManager.instance;
	private static FortressManager FortressManager => FortressManager.instance;
	private static CardVisualizationManager CardVisualizationManager => CardVisualizationManager.instance;
	private static TurnManager TurnManager => TurnManager.instance;
	private static PlayerManager PlayerManager => PlayerManager.instance;

	private static byte CurrentPlayerTurn => TurnManager.CurrentPlayerTurn;

	public static void InitializeComponents()
	{
		CardManager.Init();
		UIManager.UpdateCardNumberText(CardManager.NumberOfCardsOnDeck);
		CardExchangeController = new CardExchangeManager();
	}

	public static void OnCardTaken()
	{
		var c = CardManager.GetCardFromDeck();
        CardVisualizationManager.
			CreateCardInCorrectArea(c, CurrentPlayerTurn);
		UIManager.UpdateCardNumberText(CardManager.NumberOfCardsOnDeck);

		if (c.IsCardOnTheTable() && !CardManager.IsGameFinished)
			// Make move again
			Mediator.OnCardTaken();
	}

	public static void OnAttackStarted(byte rate)
    {
        UserStateManager.RememberUserFortSelection(rate);
        UIManager.ShowButton(UIButtons.StartAttack);
    }

    public static void OnAttackStopped() 
    {
        UserStateManager.StopCreatingOfGroupOfCharacters();
        UIManager.HideAllButtons();
        CardVisualizationManager.DeselectAllCards();
    }

	public static void OnCreatingAttackersGroupStarted()
	{
		UserStateManager.StartCreatingOfGroupOfCharacters();
		UIManager.ShowButton(UIButtons.AttackConfirmation);
	}

    public static void OnFortressTriedAttacked()
    {
		CardVisualizationManager.DeselectAllCards();

        if (!UserStateManager.CanAttackFortress)
        {
            UIManager.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        FortressManager.ProcessAttackToFortress(UserStateManager.SelectedFortIDToAttack, 
													UserStateManager.GetAttackersGroup(),
															CurrentPlayerTurn);

		UIManager.HideAllButtons();
    }

    public static void OnFortressCaptured(Fortress fort)
    {
		var attackerID = CurrentPlayerTurn;
		CardVisualizationManager.MoveCardToPlayer(fort, attackerID);
		CardVisualizationManager.RemoveAttackersFromHand(UserStateManager.GetAttackersGroup().ToList());
        CardVisualizationManager.DeselectAllCards();

		if (fort.DefendersGroup != null)
		{
			var defenderID = FortressManager.GetFortressOwner(fort.Rate);
            UIManager.ShowHint("Your fortress has been captured!\n" +
                                    "Give to attacker 1 character from hand");
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
        UserStateManager.RememberUserCharacterSelection(card);
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
		CardVisualizationManager.ShowCurrentPlayersAndHideOpponentsCards(CurrentPlayerTurn);
		PlayerManager.AssignTurn(CurrentPlayerTurn);
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
		UIManager.ShowWinnerPanel();
	}

	public static void OnSandglassAppears(Sandglass card)
	{
		CardManager.IncreaseNumberOfSandglasses();
	}

	public static void OnRuleAppears(Rule card)
	{
		throw new NotImplementedException();
	}
}