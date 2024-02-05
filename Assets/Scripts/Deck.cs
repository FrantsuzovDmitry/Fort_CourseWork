using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Deck : MonoBehaviour
	{
		private const byte CARD_NUMBER = 17;
		//public static List<Card> deck = new(CARD_NUMBER);
		public static Stack<Card> deck = new(CARD_NUMBER);

		public static Deck instance { get; private set; }

		private void Awake()
		{
			instance = this;
			CreateTestDeck();
		}

		private static void CreateRandomDeck()
		{
			//TODO: implement random deck generation
		}

		private static void CreateVanillaDeck()
		{

		}

		private static void CreateTestDeck()
		{
			/*
			string logoPath = "Sprites/" + 1;
			Sprite logo = Resources.Load<Sprite>(logoPath);
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
			deck.Add(new Fortress(2, Resources.Load<Sprite>("Sprites/37")));
			deck.Add(new Fortress(1, Resources.Load<Sprite>("Sprites/33")));
			deck.Add(new Fortress(3, Resources.Load<Sprite>("Sprites/2")));
			deck.Add(new Mirror(Resources.Load<Sprite>("Sprites/62")));
			*/
			string logoPath = "Sprites/" + 1;
			Sprite logo = Resources.Load<Sprite>(logoPath);
			deck.Push(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
			deck.Push(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
			deck.Push(new Sandglass(Resources.Load<Sprite>("Sprites/10")));
			deck.Push(new Fortress(3, Resources.Load<Sprite>("Sprites/2")));
			deck.Push(new SimpleCharacter(2, Resources.Load<Sprite>("Sprites/4")));
			deck.Push(new SimpleCharacter(1, Resources.Load<Sprite>("Sprites/5")));
			deck.Push(new SimpleCharacter(2, Resources.Load<Sprite>("Sprites/6")));
			deck.Push(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
			deck.Push(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
			deck.Push(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
			deck.Push(new Joker(Resources.Load<Sprite>("Sprites/35")));
			deck.Push(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
			deck.Push(new SimpleCharacter(3, Resources.Load<Sprite>("Sprites/9")));
			deck.Push(new Fortress(2, Resources.Load<Sprite>("Sprites/37")));
			deck.Push(new Fortress(1, Resources.Load<Sprite>("Sprites/33")));
			deck.Push(new Fortress(3, Resources.Load<Sprite>("Sprites/2")));
			deck.Push(new Mirror(Resources.Load<Sprite>("Sprites/62")));
		}
	}
}