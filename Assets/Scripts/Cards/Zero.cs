using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cards
{
	public class Zero : Character
	{
		public Zero(Sprite illustration = null) : base("Zero", illustration) { }

		public override void EnterInGroup(ref int totalForce, ref int totalWeight)
		{
			totalWeight += 1;
		}
	}
}
