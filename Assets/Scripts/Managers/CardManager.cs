using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Managers
{
	public static class CardManager
	{
		private static List<LinkedList<Card>> playersHands = new(4);
		private static List<LinkedList<Fortress>> playersForts = new(4);

		private static Deck deck = new Deck();

		public static int NumberOfCardsOnDeck => deck.deck.Count;
		public static byte NumberOfSandglasses { get; private set; }
		public static bool IsGameFinished { get; private set; }

		public static void Init()
		{
			deck.Init();
			playersHands.Clear();
			playersForts.Clear();

			for (int i = 0; i < Constants.MAX_PLAYER_ID; i++)
			{
				playersHands.Add(new LinkedList<Card>());
				playersForts.Add(new LinkedList<Fortress>());
			}

			NumberOfSandglasses = 0;
			IsGameFinished = false;
		}

		public static Card GetCardFromDeck()
		{
			var card = deck.Pop();

			card.InvokeOnCardAppearsEvent();

			if (!card.IsCardOnTheTable())
				playersHands[TurnManager.instance.CurrentPlayerTurn].AddLast(card);

			return card;
		}

		public static List<Character> GetUserHandCharacters(byte playerID)
		{
			var list = playersHands[playerID].Where(c => c is Character).ToList();
			var list2 = list.Cast<Character>().ToList();

			return list2;
		}

		public static void IncreaseNumberOfSandglasses()
		{
			NumberOfSandglasses++;
			CheckOfStopGameCondition();
		}

		private static void CheckOfStopGameCondition()
		{
			if (NumberOfSandglasses == 3)
			{
				IsGameFinished = true;
				Mediator.OnGameStopped();
			}
		}

		public static void ChangeCardOwner(Character character, byte newOwnerID)
		{
			playersHands[character.OwnerID].Remove(character);
			character.OwnerID = newOwnerID;
			playersHands[newOwnerID].AddLast(character);
		}
	}
}