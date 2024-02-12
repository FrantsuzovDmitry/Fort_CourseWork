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
using static UnityEngine.UI.Image;

namespace Assets.Scripts.Managers
{
	public static class CardManager
	{
		//private static List<List<Card>> playersHands = new(4);
		//private static List<List<Fortress>> playersForts = new(4);
		private static List<LinkedList<Card>> playersHands = new(4);
		private static List<LinkedList<Fortress>> playersForts = new(4);

		private static Deck deck = new Deck();

		public static int NumberOfCardsOnDeck => deck.deck.Count;
		public static byte NumberOfSandglasses { get; private set; }

		public static void Init()
		{
			deck.Init();
			playersHands.Clear();
			playersForts.Clear();

			for (int i = 0; i < Constants.MAX_PLAYER_ID; i++)
			{
				//playersHands.Add(new List<Card>());
				//playersForts.Add(new List<Fortress>());
				playersHands.Add(new LinkedList<Card>());
				playersForts.Add(new LinkedList<Fortress>());
			}

			NumberOfSandglasses = 0;
		}

		public static Card GetCardFromDeck()
		{
			var card = deck.Pop;

			if (card.IsCardOnTheTable())
			{
				HandleEvent(card);
			}
			else
			{
				playersHands[TurnManager.instance.CurrentPlayerTurn].AddLast(card);
			}
			return card;
		}

		private static void HandleEvent(Card card)
		{
			switch (card)
			{
				case Fortress _:
					Mediator.OnFortressAppears((Fortress)card);
					break;
				case Sandglass _:
					Mediator.OnSandglassAppears((Sandglass)card);
					break;
				case Rule _:
					Mediator.OnRuleAppears((Rule)card);
					break;
			}
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
			playersHands[newOwnerID].AddLast(character);
		}
	}
}


//ArgumentException: The Object you want to instantiate is null.
//UnityEngine.Object.CheckNullArgument (System.Object arg, System.String message) (at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//UnityEngine.Object.Instantiate(UnityEngine.Object original, UnityEngine.Transform parent, System.Boolean instantiateInWorldSpace)(at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//UnityEngine.Object.Instantiate[T](T original, UnityEngine.Transform parent, System.Boolean worldPositionStays)(at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//UnityEngine.Object.Instantiate[T](T original, UnityEngine.Transform parent)(at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//CardVisualizationManager.CreateCardInPlayerHand(Card card, System.Byte playerID)(at Assets/Scripts/Managers/CardVisualizationManager.cs:80)
//CardVisualizationManager.CreateCardInCorrectArea(Card card, System.Byte playerID)(at Assets/Scripts/Managers/CardVisualizationManager.cs:73)
//Mediator.OnCardTaken()(at Assets/Scripts/Mediator.cs:16)
//UIManager+<>c.<SetupButtons>b__15_1()(at Assets/Scripts/Managers/UIManager.cs:56)
//UnityEngine.Events.InvokableCall.Invoke()(at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//UnityEngine.Events.UnityEvent.Invoke()(at <40205bb2cb25478a9cb0f5e54cf11441>:0)
//UnityEngine.UI.Button.Press()(at Library/PackageCache/com.unity.ugui@1.0.0/Runtime/UI/Core/Button.cs:70)
//UnityEngine.UI.Button.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)(at Library/PackageCache/com.unity.ugui@1.0.0/Runtime/UI/Core/Button.cs:114)
//UnityEngine.EventSystems.ExecuteEvents.Execute(UnityEngine.EventSystems.IPointerClickHandler handler, UnityEngine.EventSystems.BaseEventData eventData)(at Library/PackageCache/com.unity.ugui@1.0.0/Runtime/EventSystem/ExecuteEvents.cs:57)
//UnityEngine.EventSystems.ExecuteEvents.Execute[T](UnityEngine.GameObject target, UnityEngine.EventSystems.BaseEventData eventData, UnityEngine.EventSystems.ExecuteEvents+EventFunction`1[T1] functor)(at Library/PackageCache/com.unity.ugui@1.0.0/Runtime/EventSystem/ExecuteEvents.cs:272)
//UnityEngine.EventSystems.EventSystem:Update()(at Library/PackageCache/com.unity.ugui@1.0.0/Runtime/EventSystem/EventSystem.cs:501)
