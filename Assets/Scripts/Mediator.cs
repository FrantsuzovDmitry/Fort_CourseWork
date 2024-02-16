using Assets.Scripts;
using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;

public static class Mediator
{
	public static void InitializeComponents()
	{
		CardManager.Init();
		UIManager.instance.UpdateCardNumberText(CardManager.NumberOfCardsOnDeck);
	}

	public static void OnCardTaken()
	{
		CardVisualizationManager.instance.
			CreateCardInCorrectArea(CardManager.GetCardFromDeck(), 
										TurnManager.instance.CurrentPlayerTurn);
		UIManager.instance.UpdateCardNumberText(CardManager.NumberOfCardsOnDeck);
	}

	public static void OnAttackStarted(byte rate)
    {
        UserStateController.RememberUserFortSelection(rate);
        UIManager.instance.ShowButton(UIButtons.StartAttack);
    }

    public static void OnAttackStopped() 
    {
        UserStateController.StopCreatingOfGroupOfCharacters();
        UIManager.instance.HideAllButtons();
        CardVisualizationManager.instance.DeselectAllCards();
    }

	public static void OnCreatingAttackersGroupStarted()
	{
		UserStateController.StartCreatingOfGroupOfCharacters();
		UIManager.instance.ShowButton(UIButtons.AttackConfirmation);
	}

    public static void OnFortressTriedAttacked()
    {
		CardVisualizationManager.instance.DeselectAllCards();

        if (!UserStateController.CanAttackFortress)
        {
            UIManager.instance.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        FortressManager.instance.
            ProcessAttackToFortress(
                UserStateController.SelectedFortIDToAttack, 
                UserStateController.GetAttackersGroup(),
                TurnManager.instance.CurrentPlayerTurn);

		UIManager.instance.HideAllButtons();
    }

    public static void OnFortressCaptured(Fortress fort, byte playerID)
    {
		CardVisualizationManager.instance.MoveCardToPlayer(fort, playerID);
		CardVisualizationManager.instance.RemoveAttackersFromHand();
        CardVisualizationManager.instance.DeselectAllCards();
		// TODO: implement logic that defender gives 1 card FROM DEFENDERS CHARACTER to attackers player
        // TODO: implement logic that defender gives his defenders back to hand
	}

	public static void OnFortressAppears(Fortress fort)
	{
		FortressManager.instance.AddNewFort(fort);
	}

	public static void OnFortressDestroyed(Fortress fort)
	{
		FortressManager.instance.RemoveFortress(fort);
	}

	public static void OnFortressUnsuccessfulAttacked()
	{
		var playerID = TurnManager.instance.CurrentPlayerTurn;
		Mediator.StartCardExchangeProcess(playerID, CardManager.GetUserHandCharacters(playerID));
		
        // TODO: implement logic that attacker gives 1 card FROM HAND to defender player
        UIManager.instance.DebugNotification("Unsuccessful attack");
	}

	public static void StartCardExchangeProcess(byte playerWhichSelectingCardID, List<Character> charactersToChoice)
	{
		UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.ON);
		UIManager.instance.ShowHint("Your attack is unsuccessful!\n" +
												"Give to defender 1 character from hand");
		CardVisualizationManager.instance.DisplayCardToChoice(playerWhichSelectingCardID, charactersToChoice);
		UserStateController.IsSelectingCardToGiveInProgress = true;
	}

	public static void OnTurnStarted()
	{
		CardVisualizationManager.instance.ShowCurrentPlayersAndHideOpponentsCards(TurnManager.instance.CurrentPlayerTurn);
		PlayerManager.instance.AssignTurn(TurnManager.instance.CurrentPlayerTurn);
		UIManager.instance.UpdateCurrentPlayerTurnLabel(TurnManager.instance.CurrentPlayerTurn);
	}

	public static void OnTurnEnded()
	{
		UIManager.instance.HideAllButtons();
		TurnManager.instance.AssignNextPlayerTurn();
		Mediator.OnTurnStarted();
	}

	public static void onGameStopped()
	{
		UIManager.instance.ShowWinnerPanel();
	}

	public static void OnCardToGiveChosen(Character card)
	{
		CardVisualizationManager.instance.DeselectAllCards();
		CardVisualizationManager.instance.SetCardRedEmission(card);
		UIManager.instance.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
	}

	public static void OnCardGiven()
	{
		UserStateController.IsSelectingCardToGiveInProgress = false;
		var fortressOwnerID = FortressManager.instance.GetFortressOwner(
									UserStateController.SelectedFortIDToAttack);

		CardManager.ChangeCardOwner(UserStateController.SelectedCharacter, fortressOwnerID);
		CardVisualizationManager.instance.
			MoveCardToPlayer(UserStateController.SelectedCharacter, fortressOwnerID);
		CardVisualizationManager.instance.DeselectAllCards();
		UIManager.instance.ToggleCardSelectionPanelVisibility(UIElementState.OFF);
		Mediator.OnTurnEnded();
	}

	public static void OnSandglassAppears(Sandglass card)
	{
		CardManager.IncreaseNumberOfSandglasses();
	}

	internal static void OnRuleAppears(Rule card)
	{
		throw new NotImplementedException();
	}
}