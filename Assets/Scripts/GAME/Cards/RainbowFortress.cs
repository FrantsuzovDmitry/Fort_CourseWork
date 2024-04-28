using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public class RainbowFortress : Fortress
    {
        public RainbowFortress(Sprite logo) : base(5, logo) { }

        public override bool ValidateAttackersGroup(GroupOfCharacters attackers)
        {
            // All characters forces must be different
            if (AreGroupContainJokers(attackers))
                return false;

            List<SimpleCharacter> simpleCharactersInAttackersGroup = attackers.SimpleCharacters;
            return AllCharactersAreDifferent(simpleCharactersInAttackersGroup);
        }

        private static bool AllCharactersAreDifferent(List<SimpleCharacter> characters)
        {
            SortByForceAscending(characters);
            int comparisonForce = -1;
            foreach (var c in characters)
            {
                if (c.Force == comparisonForce) return false;
                comparisonForce = c.Force;
            }
            return true;

            void SortByForceAscending(List<SimpleCharacter> collection) => collection.Sort((c1, c2) => c1.Force.CompareTo(c2.Force));
        }

        private static bool AreGroupContainJokers(GroupOfCharacters groupOfCharacters)
        {
            var joker = groupOfCharacters.Characters.FirstOrDefault(c => c is Joker);
            if (joker == null) return false;
            else return true;
        }
    }
}
