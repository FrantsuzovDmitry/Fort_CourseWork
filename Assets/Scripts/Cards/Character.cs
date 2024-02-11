using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.EventSystems.ExecuteEvents;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

[System.Serializable]
public class Character : Card
{
	public bool IsInGroup = false;

	public Character() : base() { }

	public Character(Character card) : base(card) { }

	public Character(string cardName, Sprite illustration) : base(cardName, illustration) { }

	public virtual void EnterInGroup(ref int totalForce, ref int totalWeight) { }

	public override NeedToBeSelected ProcessOnClick(in CardController c)
	{
		if (CurrentUserStateController.NowTheProcessOfSelectingCardToGiveAway)
		{
			if (c.IsOneOfTheCardsThatCanBeGiven())
			{
				Mediator.OnCardToGiveChosen(this);
				CurrentUserStateController.RememberUserCharacterSelection(this);
				return NeedToBeSelected.YES;
			}
			return NeedToBeSelected.NO;
		}

		if (CurrentUserStateController.NowTheProcessOfCreatingGroupIsUnderway)
		{
			if (!c.IsCardInPlayerHand())
				return NeedToBeSelected.NO;

			if (this.IsInGroup)
			{
				IsInGroup = false;
				CurrentUserStateController.RemoveCharacterFromGroup(this);
				return NeedToBeSelected.NO;
			}

			IsInGroup = true;
			CurrentUserStateController.AddCharacterToGroup(this);
			return NeedToBeSelected.YES;
		}
		return NeedToBeSelected.NO;
	}
}