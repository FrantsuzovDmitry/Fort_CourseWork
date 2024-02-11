using Assets.Scripts;
using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Constants;

/// <summary>
/// Visual cards displaying
/// </summary>
public class CardVisualizationManager : MonoBehaviour
{
	public static CardVisualizationManager instance;
	public List<Card>
			cards = new List<Card>();

	public Transform player1Hand, player2Hand, player3Hand, player4Hand,
						player1Forts, player2Forts, player3Forts, player4Forts,
						playArea, sandglassesArea;

	private Transform[] playersHandsPosition;
	private Transform[] playersFortsPosition;

	public CardController cardControllerPrefab;

	private Dictionary<Card, CardController> cardsCardControllersPairs = new(25);

	[SerializeField] public List<List<CardController>> playersCards = new();
	[SerializeField] private Transform selectingPanel;

	public short NumberOfSandglasses { get; private set; }

	private const short NotAPlayerID = 100;

	//Initialization
	private void Awake()
	{
		instance = this;
		NumberOfSandglasses = 0;
		//for (int i = 0; i < 4; i++) playersCards.Add(new List<CardController>());
	}

	private void Start()
	{
		playersFortsPosition = new Transform[4];
		playersFortsPosition[0] = player1Forts;
		playersFortsPosition[1] = player2Forts;
		playersFortsPosition[2] = player3Forts;
		playersFortsPosition[3] = player4Forts;

		playersHandsPosition = new Transform[4];
		playersHandsPosition[0] = player1Hand;
		playersHandsPosition[1] = player2Hand;
		playersHandsPosition[2] = player3Hand;
		playersHandsPosition[3] = player4Hand;
	}

	private bool IsCardOnTable(Card card)
	{
		return (card is Sandglass) || (card is Fortress) || (card is Rule);
	}

	public void CreateCardInCorrectArea(Card card, byte playerID)
	{
		if (IsCardOnTable(card))
		{
			CreateCardOnTable(card);
			Mediator.OnCardTaken();
		}
		else
		{
			CreateCardInPlayerHand(card, playerID);
		}
	}

	private void CreateCardInPlayerHand(Card card, byte playerID)
	{
		card.OwnerID = playerID;
		CardController newCard = Instantiate(cardControllerPrefab, playersHandsPosition[playerID]);
		newCard.transform.localPosition = Vector3.zero;
		newCard.Initialize(card, playerID);
		//playersCards[playerID].Add(newCard);

		// Remember the Card-CardController pairs:
		cardsCardControllersPairs.Add(card, newCard);

		return;
	}

	private void CreateCardOnTable(Card card)
	{
		CardController newCard;
		card.OwnerID = NOT_A_PLAYER_ID;
		switch (card)
		{
			case Sandglass _:
				newCard = Instantiate(cardControllerPrefab, sandglassesArea);
				IncreaseNumberOfSandglasses();
				break;

			case Fortress _:
				newCard = Instantiate(cardControllerPrefab, playArea);
				Mediator.OnFortressAppears((Fortress)card);
				break;

			case Rule _:
				throw new NotImplementedException();
				break;

			default:
				throw new NotImplementedException();
				break;
		}
		newCard.transform.localPosition = Vector3.zero;
		newCard.Initialize(card, NotAPlayerID);

		// Remember the Card-CardController pair:
		cardsCardControllersPairs.Add(card, newCard);

		return;
	}

	public void IncreaseNumberOfSandglasses()
	{
		NumberOfSandglasses++;
		CheckOfStopGameCondition();
	}

	private void CheckOfStopGameCondition()
	{
		if (NumberOfSandglasses == 3)
		{
			Mediator.onGameStopped();
		}
	}

	public void RemoveAttackersFromHand()
	{
		var existingCards = cardsCardControllersPairs.Values.ToList();
		foreach (var card in existingCards)
		{
			if (card.IsCardInPlayerHand())
				if (((Character)card.Card).IsInGroup)
				{
					cardsCardControllersPairs.Remove(card.Card);
					Destroy(card.gameObject);
				}
		}
	}

	public void ShowCurrentPlayersAndHideOpponentsCards(byte currentPlayerTurn)
	{
		foreach (var card in cardsCardControllersPairs.Keys)
		{
			if (card.OwnerID != currentPlayerTurn &&
				card.OwnerID != NOT_A_PLAYER_ID)
				cardsCardControllersPairs[card].Hide();
			else
				cardsCardControllersPairs[card].Show();
		}
	}

	public void MoveFortToPlayerArea(Fortress card, byte playerID)
	{
		cardsCardControllersPairs[card].ChangePosition(playersFortsPosition[playerID]);
	}

	public void DeselectAllCards()
	{
		foreach (CardController card in cardsCardControllersPairs.Values)
		{
			if (card.Selected)
			{
				card.MakeUnselected();
				card.SetStdEmission();
			}
		}
	}

	public void SetCardRedEmission(Character card)
	{
		cardsCardControllersPairs[card].SetSpecialEmission();
	}

	public void DisplayCardToChoice(byte playerWhichSelectingCardID, List<Character> charactersToChoice)
	{
		foreach (Character character in charactersToChoice)
		{
			cardsCardControllersPairs[character].ChangePosition(selectingPanel);
			cardsCardControllersPairs[character].transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}
}