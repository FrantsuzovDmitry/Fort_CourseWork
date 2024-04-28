using Assets.Scripts.GameEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    /// <summary>
    /// The deck that players're playing in the current round
    /// </summary>
    public class CurrentDeck
    {
        private readonly List<Card> deck = new(60);
        private readonly MainDeck _mainDeck;

        public int Count => deck.Count;

        public CurrentDeck(MainDeck mainDeck)
        {
            _mainDeck = mainDeck;
            Initialize();
        }

        public void Push(Card card)
        {
            deck.Add(card);
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
            Random rnd = new();
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
            }
        }

        public void DepersonalizeCards()
        {
            deck.ForEach(card => card.OwnerID = Constants.NOT_A_PLAYER_ID);
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
    }
}