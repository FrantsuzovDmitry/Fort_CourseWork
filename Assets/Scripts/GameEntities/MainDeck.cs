using Assets.Scripts.Cards;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameEntities
{
    public static class MainDeck
    {
        // All cards (card number = 90)
        private static Queue<Card> vanillaDeck = new(90);

        // The cards of the winning players that were in the fortress they captured are sent to this deck.
        private static Queue<Card> cardsOutOfGame = new(60);


        private class CardInfo
        {
            public string type;
            public byte parameter;
            public byte sequenceNumber;
        }

        public static Card Dequeue() => vanillaDeck.Dequeue();

        public static void Init()
        {
            vanillaDeck = DeserializeDeckFromJSON("Assets/Resources/DECK/deck.json");
        }

        // Legacy
        private static void FillTheQueue()
        {
            // 1 - 10
            //vanillaDeck.Enqueue(new Rule());
            vanillaDeck.Enqueue(new Fortress(3));
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(2));
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(2));
            //vanillaDeck.Enqueue(new Rule());
            //vanillaDeck.Enqueue(new Rule());
            vanillaDeck.Enqueue(new SimpleCharacter(3));
            vanillaDeck.Enqueue(new Sandglass());

            // 11 - 20
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(2));
            vanillaDeck.Enqueue(new SimpleCharacter(3));
            // 14 card
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(2));
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(4));
            vanillaDeck.Enqueue(new SimpleCharacter(5));
            vanillaDeck.Enqueue(new Sandglass());

            // 21 - 30
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new SimpleCharacter(3));
            vanillaDeck.Enqueue(new SimpleCharacter(4));
            vanillaDeck.Enqueue(new SimpleCharacter(8));
            vanillaDeck.Enqueue(new SimpleCharacter(5));
            vanillaDeck.Enqueue(new SimpleCharacter(4));
            vanillaDeck.Enqueue(new SimpleCharacter(5));
            vanillaDeck.Enqueue(new Sandglass());
            //vanillaDeck.Enqueue(new Rule());
            vanillaDeck.Enqueue(new TwoCharactersWithForce1());

            // 31 - 40
            vanillaDeck.Enqueue(new SimpleCharacter(2));
            vanillaDeck.Enqueue(new Joker());
            vanillaDeck.Enqueue(new Fortress(1));
            vanillaDeck.Enqueue(new SimpleCharacter(6));
            vanillaDeck.Enqueue(new Joker());
            vanillaDeck.Enqueue(new SimpleCharacter(3));
            vanillaDeck.Enqueue(new Fortress(2));
            vanillaDeck.Enqueue(new SimpleCharacter(1));
            vanillaDeck.Enqueue(new TwoCharactersWithForce1());
            vanillaDeck.Enqueue(new SimpleCharacter(2));

            // 41 - 50
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