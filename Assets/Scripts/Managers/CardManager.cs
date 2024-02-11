using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.VisualScripting.Member;
using static UnityEngine.EventSystems.ExecuteEvents;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
	public static class CardManager
	{
		private static List<List<Card>> playersHands = new(4);
		private static List<List<Fortress>> playersForts = new(4);
		private static Deck deck = new Deck();

		public static int NumberOfCardsOnDeck => deck.deck.Count;
		public static byte NumberOfSandglasses { get; private set; }

		public static void Init()
		{
			deck.Init();
			playersHands.Clear();
			playersForts.Clear();

			for(int i = 0; i < Constants.MAX_PLAYER_ID; i++)
			{
				playersHands.Add(new List<Card>());
				playersForts.Add(new List<Fortress>());
			}

			NumberOfSandglasses = 0;
		}

		public static Card GetCardFromDeck()
		{
			var card = deck.Pop;
			if (card is Character)
				playersHands[TurnManager.instance.CurrentPlayerTurn].Add(card);
			return card;
		}

		public static List<Character> GetUserHandCharacters(byte playerID)
		{
			//var list = new List<Character>();
			var list = playersHands[playerID].Where(c => c is Character).ToList();
			var list2 = list.Cast<Character>().ToList();

			return list2;
		}

		private static void IncreaseNumberOfSandglasses()
		{
			NumberOfSandglasses++;
			CheckOfStopGameCondition();
		}

		private static void CheckOfStopGameCondition()
		{
			if (NumberOfSandglasses == 3)
			{
				Mediator.onGameStopped();
			}
		}

		public static void ChangeCardOwner(Character character, byte newOwnerID)
		{
			playersHands[character.OwnerID].Remove(character);
			character.OwnerID = newOwnerID;
			playersHands[newOwnerID].Add(character);
		}
	}
}