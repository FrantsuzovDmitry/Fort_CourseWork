using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Constants;

/// <summary>
/// Visual cards displaying
/// </summary>
public class CardVisualizationManager : MonoBehaviour
{
	public static CardVisualizationManager instance;

	private Transform[] playersHandsPosition;
	private Transform[] playersFortsPosition;

	private Dictionary<Card, CardController> cardsCardControllersPairs = new(25);

	[SerializeField] private CardController cardControllerPrefab;
	[SerializeField] private Transform selectingPanel;
	[SerializeField] private Transform player1Hand, player2Hand, player3Hand, player4Hand,
										player1Forts, player2Forts, player3Forts, player4Forts,
										playArea, sandglassesArea;

	//Initialization
	private void Awake()
	{
		instance = this;
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

	public void OnCardTaken(Card card, byte playerID) => CreateCardInCorrectArea(card, playerID);

	public void OnFortressDestroyed(Fortress fortress)
	{
		Destroy(cardsCardControllersPairs[fortress].gameObject);
		cardsCardControllersPairs.Remove(fortress);
		//cardsCardControllersPairs[fortress].gameObject.Destroy();
	}

    private void CreateCardInCorrectArea(Card card, byte playerID)
	{
		if (card.CardShouldBeOnTheTable())
		{
			CreateCardOnTable(card);
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
		newCard.Initialize(card);

		// Remember the Card-CardController pairs:
		cardsCardControllersPairs.Add(card, newCard);

		return;
	}

	public void MakeCardSelected(Card card)
	{
		cardsCardControllersPairs[card].MakeSelected();
	}

	private void CreateCardOnTable(Card card)
	{
		CardController newCard;
		card.OwnerID = NOT_A_PLAYER_ID;
		switch (card)
		{
			case Sandglass _:
				newCard = Instantiate(cardControllerPrefab, sandglassesArea);
				break;

			case Fortress _:
				newCard = Instantiate(cardControllerPrefab, playArea);
				break;

			case Rule _:
				throw new NotImplementedException();
				break;

			default:
				throw new NotImplementedException();
				break;
		}
		newCard.transform.localPosition = Vector3.zero;
		newCard.Initialize(card);

		// Remember the Card-CardController pair:
		cardsCardControllersPairs.Add(card, newCard);

		return;
	}

	public void RemoveAttackersFromHand(List<Character> attackersGroup)
	{
		foreach (var card in attackersGroup)
		{
			MakeInvisible(cardsCardControllersPairs[card]);
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

	public void MoveCardToPlayer(Card card, byte playerID)
	{
		var cardController = cardsCardControllersPairs[card];

        if (card is Fortress)
			cardController.ChangePosition(playersFortsPosition[playerID]);
		else if (card is Character)
			cardController.ChangePosition(playersHandsPosition[playerID]);

		MakeVisible(cardController);
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

	public void DisplayCardsToChoice(List<Character> charactersToChoice)
	{
		foreach (Character character in charactersToChoice)
		{
			var card = cardsCardControllersPairs[character];

            card.ChangePosition(selectingPanel);
			card.transform.localRotation = Quaternion.Euler(0, 0, 0);
			MakeVisible(card);
		}
	}

	private void MakeVisible(CardController card)
	{
		card.gameObject.SetActive(true);
		card.Show();
	}

	private void MakeInvisible(CardController card)
	{
		card.gameObject.SetActive(false);
	}
}