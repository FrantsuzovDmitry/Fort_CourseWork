using UnityEngine;

namespace Assets.Scripts.Cards
{
	public class TwoCharactersWithForce1 : SimpleCharacter
	{
		public new int Force = 1;

		public TwoCharactersWithForce1(Sprite illustration=null) : base(1, illustration) { }

		public override void EnterInGroup(ref int totalForce, ref int totalWeight)
		{
			// Twice
			base.EnterInGroup(ref totalForce, ref totalWeight);
			base.EnterInGroup(ref totalForce, ref totalWeight);
		}
	}
}
