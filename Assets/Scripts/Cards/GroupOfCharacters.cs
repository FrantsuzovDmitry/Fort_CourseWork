using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Cards
{
	public class GroupOfCharacters
	{
		public List<Character> Characters { get; }
		public List<SimpleCharacter> SimpleCharacters { get; private set; }
		public int TotalForce { get; private set; }
		public int CardInGroup => Characters.Count;

		public GroupOfCharacters(List<Character> characters)
		{
			TotalForce = CalculateGroupForce(characters);
			Characters = characters;
		}

		private void SortGroup(List<Character> characters)
		{
			List<Character> simpleCharacters = new List<Character>();
			List<Character> mirrors = new List<Character>(3);
			List<Character> jokers = new List<Character>(3);

			// Jokers must be after simple characters
			// Jokers must be before nulls
			// Mirrors must be after nulls
			foreach (Character character in characters)
			{
				if (character is SimpleCharacter)
					simpleCharacters.Add(character);
				else if (character is Joker)
					jokers.Add(character);
				else mirrors.Add(character);
			}
			characters = simpleCharacters;
			characters.AddRange(jokers);
			characters.AddRange(mirrors);
		}

		private int CalculateGroupForce(List<Character> characters)
		{
			SortGroup(characters);
			int totalCurrentForce = 0, totalWeight = 0;
			foreach (Character character in characters)
				character.EnterInGroup(ref totalCurrentForce, ref totalWeight);

			return totalCurrentForce * totalWeight;
		}

		//TODO: Удалить это здесь (и референс тоже исправить), и переместить в Fortress requirements
		private bool AreCharactersForcesEqual(List<SimpleCharacter> characters)
		{
			int comparisonForce = characters[0].Force;
			for (int i = 1; i < characters.Count; i++)
				if (comparisonForce != characters[i].Force) 
					return false;
			return true;
		}
	}
}
