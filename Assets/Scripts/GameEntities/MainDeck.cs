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
        private static Queue<Card> vanillaDeck = new(90);
        // The cards of the winning players that were in the fortress they captured are sent to this deck.
        private static Queue<Card> cardsOutOfGame = new(60);

        private const string PathToJSON = "Assets/Resources/DECK/deck.json";

        private class CardInfo
        {
            public string type;
            public byte parameter;
            public byte sequenceNumber;
        }

        public static Card Dequeue() => vanillaDeck.Dequeue();

        public static void Init()
        {
            vanillaDeck = DeserializeDeckFromJSON(PathToJSON);
        }

        private static Queue<Card> DeserializeDeckFromJSON(string filename)
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
                    "Rule" => new Rule(),
                    "Fortress" => new Fortress(parameter),
                    "SimpleCharacter" => new SimpleCharacter(parameter),
                    "Sandglass" => new Sandglass(),
                    "Joker" => new Joker(),
                    "TwoCharactersWithForce1" => new TwoCharactersWithForce1(),
                    "Mirror" => new Mirror(),
                    "Zero" => new Zero(),
                    "RainbowFortress" => new RainbowFortress(),
                    "Wizard" => new Wizard(),

                    _ => throw new ArgumentException($"Unknown card type: {type}"),
                };
                card.SetLogo(cardLogo);

                mainDeck.Enqueue(card);
            }
            return mainDeck;
        }
        public static void RemoveUsedCardFromDeck(Card card)
        {
            cardsOutOfGame.Enqueue(card);
        }
    }
}