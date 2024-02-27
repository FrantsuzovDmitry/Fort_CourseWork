using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Scripts;

public enum NeedToBeSelected : byte
{
	YES, NO
}

public class CardController : MonoBehaviour, /*IPointerEnterHandler, IPointerExitHandler, */IPointerDownHandler
{
	public Card Card;
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private Image illustration, standardEmission, specialEmission, currentEmission, cardBack;

	public bool Selected { get; private set; } = false;

	private Transform parentPosition;
	private Vector3 Position => transform.position;

	public void Initialize(Card card, int ownerID)
	{
		this.Card = card;
		this.Card.OwnerID = card.OwnerID;
		illustration.sprite = card.illustration;
		cardName.text = card.cardName;

		cardBack.SetInactive();

		parentPosition = transform.parent;

		currentEmission = standardEmission;
	}

	/// Turn ON card back
	public void Hide()
	{
		cardBack.SetActive();
	}

	/// Turn OFF card back
	public void Show()
	{
		cardBack.SetInactive();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Card.ProcessOnClick(this) == NeedToBeSelected.YES)
			MakeSelected();
		else
			MakeUnselected();
	}

	public bool IsOneOfTheCardsThatCanBeGiven()
	{
		return parentPosition.name ==
				$"CardsToSelectionPanel";
	}

	public void SetStdEmission()
	{
		currentEmission = standardEmission;
	}

	public void SetSpecialEmission()
	{
		currentEmission = specialEmission;
	}

	public void MakeSelected()
	{
		currentEmission.SetActive();
		//standardEmission.SetActive();
		transform.position = new Vector3(Position.x, Position.y, -5);   // set in front
		Selected = true;
	}

	public void MakeUnselected()
	{
		currentEmission.SetInactive();
		//standardEmission.SetInactive();
		transform.position = new Vector3(Position.x, Position.y, 0);    // default value
		Selected = false;
	}

	public bool IsCardInPlayerHand()
	{
		return parentPosition.name ==
				$"Player{TurnManager.instance.CurrentPlayerTurn + 1}Hand";
	}

	public bool IsCardIsPlayersOwn()
	{
		return parentPosition.name ==
				$"Player{TurnManager.instance.CurrentPlayerTurn + 1}Forts";
	}

	public bool IsCardInTheMidOfTable() { return parentPosition.name == "PlayArea"; }

	public void ChangePosition(Transform parent)
	{
		this.gameObject.transform.SetParent(parent);
        parentPosition = parent;
	}

	private void ReturnToHand()
	{
		//transform.SetParent(originalParent);
		//transform.localPosition = Vector3.zero;
	}
}
