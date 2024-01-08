using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Cards
{
	public class RainbowFortress : Fortress
	{
		public override bool IsRequirementsToDefendersAreAccept(GroupOfCharacters groupOfCharacters)
		{
			if (AreGroupContainJokers(groupOfCharacters))
				return false;

			// All characters forces must be different

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
