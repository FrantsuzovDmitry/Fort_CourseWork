using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public  class Wizard: Character
    {
        public Wizard(Sprite illustration) : base("wizard", illustration) { }

        public override void EnterInGroup(ref int totalForce, ref int totalWeight)
        {
            // TODO: (last) Make that only this card can be in the group
            totalForce = int.MaxValue;
            totalWeight = 1;
        }
    }
}
