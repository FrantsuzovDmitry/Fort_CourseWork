using System;
using System.Linq;

namespace Assets.Scripts.Cards
{
	public class RainbowFortress : Fortress
	{
		public override bool IsRequirementsToDefendersAreAccept(GroupOfCharacters groupOfCharacters)
		{
			// All characters forces must be different

			if (AreGroupContainJokers(groupOfCharacters))
				return false;

			//TODO: Check this predicate
			return !base.IsRequirementsToDefendersAreAccept (groupOfCharacters);
		}

		private bool AreGroupContainJokers(GroupOfCharacters groupOfCharacters)
		{
			var joker = groupOfCharacters.Characters.FirstOrDefault(c => c is Joker);
			if (joker == null) return false;
			else return true;
		}
	}
}
