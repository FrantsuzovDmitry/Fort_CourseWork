using Assets.Scripts;
using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Mediator
{
	public static void OnCardTaken()
	{
		CardVisualizationManager.instance.TakeCardFromDeck(TurnManager.instance.currentPlayerTurn);
		GameplayUIController.instance.UpdateCardNumberText(Deck.deck.Count.ToString());
	}

	public static void OnAttackStarted(byte rate)
    {
        CurrentUserStateController.RememberUserFortSelection(rate);
        UIManager.instance.ShowButton(UIButtons.StartAttackButton);
    }

    public static void OnAttackStopped() 
    {
        CurrentUserStateController.StopCreatingOfGroupOfCharacters();
        UIManager.instance.HideCurrentButtons();
        CardVisualizationManager.instance.DeselectAllCards();
    }

	public static void OnCreatingAttackersGroupStarted()
	{
		CurrentUserStateController.StartCreatingOfGroupOfCharacters();
		UIManager.instance.ShowButton(UIButtons.AttackConfirmationButton);
	}

    public static void OnFortressTriedAttacked()
    {
        if (!CurrentUserStateController.CanAttackFortress)
        {
            UIManager.instance.ShowWarningMessage("Выберите хотя бы одного атакующего!");
            return;
        }

        FortressManager.instance
            .ProcessAttackToFortress(
                CurrentUserStateController.SelectedFortToAttack, 
                CurrentUserStateController.GetAttackersGroup(),
                TurnManager.instance.currentPlayerTurn);
    }

    public static void OnFortressCaptured(Fortress fort, byte playerID)
    {
		CardVisualizationManager.instance.ChangeParentPosition(fort, playerID);
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
        // TODO: implement logic that attacker gives 1 card FROM HAND to defender player
        UIManager.instance.DebugNotification("Unsuccessful attack");
	}

    public static void onGameStopped()
    {
        UIManager.instance.ShowWinnerPanel();
	}
}