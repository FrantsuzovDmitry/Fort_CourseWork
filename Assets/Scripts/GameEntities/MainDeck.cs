using Assets.Scripts.Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameEntities
{
    public class MainDeck
    {
        // All cards (card number = 90)
        private readonly Queue<Card> vanillaDeck;
        // The cards of the winning players that were in the fortress they captured are sent to this deck.
        private readonly Queue<Card> cardsOutOfGame = new(60);

        private const string PathToJSON = "Assets/Resources/DECK/deck.json";

        private class CardInfo
        {
            public string type;
            public byte parameter;
            public byte sequenceNumber;
        }

        public Card Dequeue() => vanillaDeck.Dequeue();

        public MainDeck()
        {
            vanillaDeck = DeserializeDeckFromJSON(PathToJSON);
        }

        private Queue<Card> DeserializeDeckFromJSON(string filename)
        {
            string jsonText = System.IO.File.ReadAllText(filename);
            List<CardInfo> listOfCardData = JsonConvert.DeserializeObject<List<CardInfo>>(jsonText);
            Queue<Card> mainDeck = new Queue<Card>(90);

            foreach (var cardData in listOfCardData)
            {
                string type = cardData.type;
                byte parameter = cardData.parameter;
                Sprite cardLogo =
                    Resources.Load<Sprite>("Sprites/" + cardData.sequenceNumber);

                Card card = cardData.type switch
                {
                    "Rule" => new Rule(cardLogo),
                    "Fortress" => new Fortress(parameter, cardLogo),
                    "SimpleCharacter" => new SimpleCharacter(parameter, cardLogo),
                    "Sandglass" => new Sandglass(cardLogo),
                    "Joker" => new Joker(cardLogo),
                    "TwoCharactersWithForce1" => new TwoCharactersWithForce1(cardLogo),
                    "Mirror" => new Mirror(cardLogo),
                    "Zero" => new Zero(cardLogo),
                    "RainbowFortress" => new RainbowFortress(cardLogo),
                    "Wizard" => new Wizard(cardLogo),

                    _ => throw new ArgumentException($"Unknown card type: {type}"),
                };

                mainDeck.Enqueue(card);
            }
            return mainDeck;
        }
        public void RemoveUsedCardFromDeck(Card card)
        {
            cardsOutOfGame.Enqueue(card);
        }
    }
}