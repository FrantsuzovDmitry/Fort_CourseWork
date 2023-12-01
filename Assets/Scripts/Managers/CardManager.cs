using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card>
            cards = new List<Card>(),
            deck = new List<Card>();
    public Transform player1Hand, player2Hand, player3Hand, player4Hand,
                        player1Forts, player2Forts, player3Forts, player4Forts,
                        playArea, sandglassesArea;

    private Transform[] playerHandsPosition;
    private Transform[] playersFortsPosition;

    public CardController cardControllerPrefab;

    // For debug 
    public List<CardController>
            player1Cards = new List<CardController>(),
            player2Cards = new List<CardController>(),
            player3Cards = new List<CardController>(),
            player4Cards = new List<CardController>();

    [SerializeField] public List<List<CardController>> playersCards = new List<List<CardController>>();
    [SerializeField] private List<Character> groupOfCharacters = new List<Character>();

    public int numberOfSandglasses;

    private const int NotAPlayerID = 100;

    public List<Character> GroupOfCharacters { get => groupOfCharacters; }

    //Initialization
    private void Awake()
    {
        instance = this;
        numberOfSandglasses = 0;
        for (int i = 0; i < 4; i++) playersCards.Add(new List<CardController>());
    }

    private void Start()
    {
        GenerateDeck();

        playersFortsPosition = new Transform[4];
        playersFortsPosition[0] = player1Forts;
        playersFortsPosition[1] = player2Forts;
        playersFortsPosition[2] = player3Forts;
        playersFortsPosition[3] = player4Forts;

        playerHandsPosition = new Transform[4];
        playerHandsPosition[0] = player1Hand;
        playerHandsPosition[1] = player2Hand;
        playerHandsPosition[2] = player3Hand;
        playerHandsPosition[3] = player4Hand;

        player1Cards = playersCards[0];
        player2Cards = playersCards[1];
        player3Cards = playersCards[2];
        player4Cards = playersCards[3];

    }

    private void GenerateDeck()
    {
        // Real deck (for now is locked)
        /*
        for (int i = 0; i < 10; i++)
        {
            string logoPath = "Card pictures/" + (i + 1);
            Sprite logo = Resources.Load<Sprite>(logoPath);
            var ClassName = CardDatabase.CardTemplates[i].ClassName;
            var force = CardDatabase.CardTemplates[i].force;
            var weight = CardDatabase.CardTemplates[i].weight;
            var rate = CardDatabase.CardTemplates[i].rate;

            switch (CardDatabase.CardTemplates[i].ClassName)
            {
                case "Rule":
                    //deck.Add(new Rule(logoPath));
                    break;
                case "Fort":
                    deck.Add(new Fort());
                    break;
                case "Character":

                    break;
                case "Sandglasses":

                    break;
            }
        }
        */

        // Test deck
        string logoPath = "Sprites/" + 1;
        Sprite logo = Resources.Load<Sprite>(logoPath);
        deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
        deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
        deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
        deck.Add(new Fortress(3, Resources.Load<Sprite>("Sprites/2")));
        deck.Add(new SimpleCharacter(2, 1, 1, Resources.Load<Sprite>("Sprites/4")));
        deck.Add(new SimpleCharacter(1, 1, 1, Resources.Load<Sprite>("Sprites/5")));
        deck.Add(new SimpleCharacter(2, 1, 1, Resources.Load<Sprite>("Sprites/6")));
        deck.Add(new SimpleCharacter(3, 1, 1, Resources.Load<Sprite>("Sprites/9")));
        deck.Add(new SimpleCharacter(3, 1, 1, Resources.Load<Sprite>("Sprites/9")));
        deck.Add(new SimpleCharacter(3, 1, 1, Resources.Load<Sprite>("Sprites/9")));
        deck.Add(new Joker(Resources.Load<Sprite>("Sprites/35")));
        deck.Add(new SimpleCharacter(3, 1, 1, Resources.Load<Sprite>("Sprites/9")));
        deck.Add(new SimpleCharacter(3, 1, 1, Resources.Load<Sprite>("Sprites/9")));
        deck.Add(new Fortress(1, Resources.Load<Sprite>("Sprites/2")));
        deck.Add(new Fortress(1, Resources.Load<Sprite>("Sprites/2")));
        deck.Add(new Fortress(1, Resources.Load<Sprite>("Sprites/2")));
        deck.Add(new Mirror(Resources.Load<Sprite>("Sprites/62")));
    }

    public void GiveCardToPlayer(int playerID)
    {
        Card card = deck[deck.Count - 1];
        CardController newCard;
        var type = card.GetType();

        if (card is Sandglass || card is Fortress || card is Rule)
        {
            if (card is Sandglass)
            {
                newCard = Instantiate(cardControllerPrefab, sandglassesArea);
                IncreaseNumberOfSandglasses();
            }
            else //(card is Fortress)
            {
                newCard = Instantiate(cardControllerPrefab, playArea);
                FortressManager.instance.AddFort(card);
            }
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, NotAPlayerID);
        }
        else
        {
            newCard = Instantiate(cardControllerPrefab, playerHandsPosition[playerID]);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, playerID);

            playersCards[playerID].Add(newCard);
        }

        // Delete top card
        deck.RemoveAt(deck.Count - 1);
    }

    private void GenerateCards()
    {
        foreach (Card card in deck)
        {
            CardController newCard = Instantiate(cardControllerPrefab, player1Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 0);
        }
    }

    public void IncreaseNumberOfSandglasses()
    {
        numberOfSandglasses++;
        CheckOfStopGameCondition();
    }

    private void CheckOfStopGameCondition()
    {
        if (numberOfSandglasses == 3)
        {
            Observer.onGameStopped();
            //PlayerManager.instance.EndGame();
        }
    }

    public void AddCharacterToGroup(Character character)
    {
        groupOfCharacters.Add(character);
    }

    public void RemoveCharacterFromGroup(Character character)
    {
        groupOfCharacters.Remove(character);
    }

    public void StopCreatingOfGroup()
    {
        groupOfCharacters.Clear();
    }

    public void RemoveAttackersFromHand()
    {
        int i = 0;
        var playerHand = playersCards[TurnManager.instance.currentPlayerTurn];
        while (i < playerHand.Count)
        {
            var card = playerHand[i];
            // if card was in the attackers group, the card will be removed
            if (((Character)card.card).isInGroup)
            {
                playerHand.Remove(card);
                Destroy(card.gameObject);
            }
            else
            {
                i++;
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

    public void ShowMyCards()
    {
        foreach (CardController card in playersCards[TurnManager.instance.currentPlayerTurn])
            card.cardBack.gameObject.SetActive(false);
    }

    public void ChangeParentPosition(CardController card)
    {
        card.gameObject.transform.parent = playersFortsPosition[TurnManager.instance.currentPlayerTurn];
        card.changeParent(playersFortsPosition[TurnManager.instance.currentPlayerTurn]);
    }

    private void OnEnable()
    {
        TurnManager.instance.onAttackStopped += StopCreatingOfGroup;
    }

    private void OnDisable()
    {

    }
}