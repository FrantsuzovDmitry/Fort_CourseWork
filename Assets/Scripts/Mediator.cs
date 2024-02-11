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
		CardVisualizationManager.instance
			.CreateCardInCorrectArea(CardManager.GetCardFromDeck(), 
										TurnManager.instance.CurrentPlayerTurn);
		UIManager.instance.UpdateCardNumberText(CardManager.NumberOfCardsOnDeck);
	}

	public static void OnAttackStarted(byte rate)
    {
        CurrentUserStateController.RememberUserFortSelection(rate);
        UIManager.instance.ShowButton(UIButtons.StartAttack);
    }

    public static void OnAttackStopped() 
    {
        CurrentUserStateController.StopCreatingOfGroupOfCharacters();
        UIManager.instance.HideAllButtons();
        CardVisualizationManager.instance.DeselectAllCards();
    }

	public static void OnCreatingAttackersGroupStarted()
	{
		CurrentUserStateController.StartCreatingOfGroupOfCharacters();
		UIManager.instance.ShowButton(UIButtons.AttackConfirmation);
	}

    public static void OnFortressTriedAttacked()
    {
		CardVisualizationManager.instance.DeselectAllCards();

        if (!CurrentUserStateController.CanAttackFortress)
        {
            UIManager.instance.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        FortressManager.instance
            .ProcessAttackToFortress(
                CurrentUserStateController.SelectedFortToAttack, 
                CurrentUserStateController.GetAttackersGroup(),
                TurnManager.instance.CurrentPlayerTurn);

		UIManager.instance.HideAllButtons();
    }

    public static void OnFortressCaptured(Fortress fort, byte playerID)
    {
		CardVisualizationManager.instance.MoveFortToPlayerArea(fort, playerID);
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
		StartCardExchangeProcess(playerID, CardManager.GetUserHandCharacters(playerID));
		
        // TODO: implement logic that attacker gives 1 card FROM HAND to defender player
        UIManager.instance.DebugNotification("Unsuccessful attack");
	}

	public static void StartCardExchangeProcess(byte playerWhichSelectingCardID, List<Character> charactersToChoice)
	{
		UIManager.instance.BlurBackground();
		UIManager.instance.ShowHint("Your attack is unsuccessful!\n" +
												"Give to defender 1 character from hand");
		CardVisualizationManager.instance.DisplayCardToChoice(playerWhichSelectingCardID, charactersToChoice);
		CurrentUserStateController.NowTheProcessOfSelectingCardToGiveAway = true;
	}

	public static void OnTurnEnded()
	{
		UIManager.instance.HideAllButtons();
		TurnManager.instance.AssignNextPlayerTurn();
		Mediator.OnTurnStarted();
	}

	public static void OnTurnStarted()
	{
		CardVisualizationManager.instance.ShowCurrentPlayersAndHideOpponentsCards(TurnManager.instance.CurrentPlayerTurn);
		PlayerManager.instance.AssignTurn(TurnManager.instance.CurrentPlayerTurn);
		UIManager.instance.UpdateCurrentPlayerTurnLabel(TurnManager.instance.CurrentPlayerTurn);
	}

	public static void onGameStopped()
	{
		UIManager.instance.ShowWinnerPanel();
	}

	public static void OnCardGiven()
	{
		CurrentUserStateController.NowTheProcessOfSelectingCardToGiveAway = false;
		CardManager.ChangeCardOwner(CurrentUserStateController.selectedCharacter,
								TurnManager.instance.CurrentPlayerTurn);
	}

	public static void OnCardToGiveChosen(Character card)
	{
		CardVisualizationManager.instance.DeselectAllCards();
		CardVisualizationManager.instance.SetCardRedEmission(card);
		UIManager.instance.ShowButton(UIButtons.SelectionCharacterToGiveConfirmation);
	}
}