using Assets.Scripts.GameEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Managers
{
	public class CardManager
	{
		private readonly List<LinkedList<Card>> playersHands = new(4);

		private readonly CurrentDeck currentDeck;
		public int NumberOfCardsInDeck => currentDeck.deck.Count;

		public CardManager()
		{
			currentDeck = new CurrentDeck(new MainDeck());
            currentDeck.Init();
			playersHands.Clear();

			for (int i = 0; i < Constants.MAX_PLAYER_ID + 1; i++)
			{
				playersHands.Add(new LinkedList<Card>());
			}
		}

		public Card GetCardFromDeck()
		{
			var card = currentDeck.Pop();

			if (!card.CardShouldBeOnTheTable())
				playersHands[TurnManager.instance.CurrentPlayerTurn].AddLast(card);

			return card;
		}

		public List<Character> GetUserHandCharacters(byte playerID)
		{
			var list = playersHands[playerID].Where(c => c is Character).ToList();
			var list2 = list.Cast<Character>().ToList();

			return list2;
		}

		public void ChangeCardOwner(Character character, byte newOwnerID)
		{
			playersHands[character.OwnerID].Remove(character);
			character.OwnerID = newOwnerID;
			playersHands[newOwnerID].AddLast(character);
		}

        public void GenerateNewDeck(List<Card> cardsToRemove)
        {
            currentDeck.RemoveCardsFromDeck(cardsToRemove);
			currentDeck.AddFiveCardFromMainDeck();
			currentDeck.Shuffle();
		}

        public void OnGameStopped()
        {
			// Reset cards owners
            foreach (var card in currentDeck.deck)
                card.OwnerID = Constants.NOT_A_PLAYER_ID;
        }
    }
}