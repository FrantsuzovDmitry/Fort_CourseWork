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

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public Card Card;
	public Image illustration, emission, cardBack;
	public TextMeshProUGUI cardName;
	public bool Selected { get; private set; } = false;

	private Transform parentPosition;
	private Vector3 Position => transform.position;

	public void Initialize(Card card, int ownerID)
	{
		this.Card = card;
		this.Card.ownerID = card.ownerID;
		illustration.sprite = card.illustration;
		cardName.text = card.cardName;

		cardBack.SetInactive();

		parentPosition = transform.parent;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{

	}

	public void OnPointerExit(PointerEventData eventData)
	{

	}

	private void ShowCardInfo()
	{
		//TODO:
		/*
         * При наведении (или при долгом нажатии) на карту должна появляться информация про неё.
         * То есть должна дергаться инфа о карте из класса Card. Что-то типа GetCardInformation; 
         * Это сильно потом надо будет сделать.
         * 
         * При обычном нажатии происходит выбор карты
         * */
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Card.ProcessOnClick(this) == NeedToBeSelected.YES)
			MakeSelected();
		else
			MakeUnselected();
	}

	public void MakeSelected()
	{
		emission.SetActive();
		transform.position = new Vector3(Position.x, Position.y, -5);   // set in front
		Selected = true;
	}

	public void MakeUnselected()
	{
		emission.SetInactive();
		transform.position = new Vector3(Position.x, Position.y, 0);    // default value
		Selected = false;
	}

	public bool IsCardInPlayerHand()
	{
		return parentPosition.name ==
				$"Player{TurnManager.instance.currentPlayerTurn + 1}Hand";
	}

	public bool IsCardIsPlayersOwn()
	{
		return parentPosition.name ==
				$"Player{TurnManager.instance.currentPlayerTurn + 1}Forts";
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
