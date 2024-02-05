using Assets.Scripts;
using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class CardVisualizationManager : MonoBehaviour
{
	public static CardVisualizationManager instance;
	public List<Card>
			cards = new List<Card>();
			//deck = new List<Card>();

	private Stack<Card> deck = Deck.deck;
	public Transform player1Hand, player2Hand, player3Hand, player4Hand,
						player1Forts, player2Forts, player3Forts, player4Forts,
						playArea, sandglassesArea;

	private Transform[] playersHandsPosition;
	private Transform[] playersFortsPosition;

	public CardController cardControllerPrefab;

	private Dictionary<Card, CardController> cardsCardControllersPairs = new(25);

	// For debug 
	public List<CardController>
			player1Cards = new(),
			player2Cards = new(),
			player3Cards = new(),
			player4Cards = new();

	[SerializeField] public List<List<CardController>> playersCards = new();

	[SerializeField] private GroupOfCharacters groupOfCharacters;

	public short NumberOfSandglasses { get; private set; }

	public short NumberOfCardInDeck { get => (short)deck.Count; }

	public GroupOfCharacters GroupOfCharacters { get => groupOfCharacters; }

	private const short NotAPlayerID = 100;

	//Initialization
	private void Awake()
	{
		instance = this;
		NumberOfSandglasses = 0;
		for (int i = 0; i < 4; i++) playersCards.Add(new List<CardController>());
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

		player1Cards = playersCards[0];
		player2Cards = playersCards[1];
		player3Cards = playersCards[2];
		player4Cards = playersCards[3];

	}

	private bool IsCardOnTable(Card card)
	{
		return (card is Sandglass) || (card is Fortress) || (card is Rule);
	}

	private void CreateCardInCorrectArea(Card card, int playerID)
	{
		if (IsCardOnTable(card))
		{
			CreateCardOnTable(card);
			//TODO: YOU CAN MAKE MOVE AGAIN
		}
		else
		{
			CreateCardInPlayerHand(card, playerID);
		}
	}

	private void CreateCardInPlayerHand(Card card, int playerID)
	{
		CardController newCard = Instantiate(cardControllerPrefab, playersHandsPosition[playerID]);
		newCard.transform.localPosition = Vector3.zero;
		newCard.Initialize(card, playerID);
		playersCards[playerID].Add(newCard);

		// Remember the Card-CardController pairs:
		cardsCardControllersPairs.Add(card, newCard);

		return;
	}

	private void CreateCardOnTable(Card card)
	{
		CardController newCard;
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

	public void TakeCardFromDeck(int playerID)
	{
		if (!deck.TryPop(out Card card))
			return;

		CreateCardInCorrectArea(card, playerID);

		if (IsCardOnTable(card))
			TakeCardFromDeck(playerID);
		//else
		//TurnManager.instance.EndTurn();				////////////////////////////////
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

	public void HideOpponentsCards()
	{
		for (int i = 0; i < playersCards.Count; i++)
			if (i != TurnManager.instance.currentPlayerTurn)
				foreach (CardController card in playersCards[i])
					card.cardBack.gameObject.SetActive(true);
	}

	public void ShowCurrentPlayerCards()
	{
		foreach (CardController card in playersCards[TurnManager.instance.currentPlayerTurn])
			card.cardBack.gameObject.SetActive(false);
	}

	public void ChangeParentPosition(Fortress card, byte playerID)
	{
		cardsCardControllersPairs[card].ChangePosition(playersFortsPosition[playerID]);
		//cardsCardControllersPairs[card].gameObject.transform.parent = playersFortsPosition[TurnManager.instance.currentPlayerTurn];
	}

	public void DeselectAllCards()
	{
		foreach (CardController card in cardsCardControllersPairs.Values)
		{
			if (card.Selected)
				card.MakeUnselected();
		}
	}
}