using Assets.Scripts.GameEntities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The deck that players're playing in the current round
    /// </summary>
    public class CurrentDeck
    {
        public List<Card> deck = new(60);

        private readonly MainDeck _mainDeck;

        public int Count => deck.Count;

        public CurrentDeck(MainDeck mainDeck)
        {
            _mainDeck = mainDeck;
            Initialize();
        }

        public Card Pop()
        {
            if (deck.Count != 0)
            {
                return Get();
            }
            else return null;
        }

        public void AddFiveCardFromMainDeck()
        {
            for (int i = 0; i < 5; i++)
                Add(_mainDeck.Dequeue());
        }

        public void Shuffle()
        {
            Card card;
            System.Random rnd = new ();
            for (int i = 0; i < deck.Count; i++)
            {
                var index = rnd.Next(i, deck.Count);
                card = deck[i];
                deck[i] = deck[index];
                deck[index] = card;
            }
        }

        public void RemoveCardsFromDeck(List<Card> cards)
        {
            foreach (var card in cards)
            {
                _mainDeck.RemoveUsedCardFromDeck(card);
                deck.Remove(card);
            }
        }

        private void Add(Card card) => deck.Add(card);

        private Card Get()
        {
            var card = deck.Last();
            deck.Remove(card);
            return card;
        }

        private void Initialize()
        {
            CreateStartDeck();
        }

        private void CreateStartDeck()
        {
            const int numOf3rdSasdglassInDeck = 28;
            List<int> cardsNumbersToExclude = new List<int>() { 1, 7, 8, 14 };
            for (int i = 0;
                i < numOf3rdSasdglassInDeck - cardsNumbersToExclude.Count;
                i++)
                Add(_mainDeck.Dequeue());

            deck.Reverse();
        }

        private void CreateRandomDeck()
        {
            //TODO: (last)implement random deck generation
        }

        private void CreateTestDeck()
        {
            //string logoPath = "Sprites/" + 1;
            //Sprite logo = Resources.Load<Sprite>(logoPath);
            deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
            deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
            deck.Add(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
            deck.Add(new Fortress(3, Resources.Load<Sprite>("Sprites/2")));
            deck.Add(new SimpleCharacter(2, Resources.Load<Sprite>("Sprites/4")));
            deck.Add(new SimpleCharacter(1, Resources.Load<Sprite>("Sprites/5")));
            deck.Add(new SimpleCharacter(2, Resources.Load<Sprite>("Sprites/6")));
            deck.Add(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
            deck.Add(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
            deck.Add(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
            deck.Add(new Joker(Resources.Load<Sprite>("Sprites/35")));
            deck.Add(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
            deck.Add(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
            deck.Add(new Fortress(3, Resources.Load<Sprite>("Sprites/37")));
            deck.Add(new Fortress(2, Resources.Load<Sprite>("Sprites/33")));
            deck.Add(new Fortress(1, Resources.Load<Sprite>("Sprites/2")));
            deck.Add(new Mirror(Resources.Load<Sprite>("Sprites/62")));
        }
    }
}