using System.Collections.Generic;

namespace Assets.Scripts.AI.AIActions
{
    public class AttackTheFortress : AIAction
    {
        public readonly Fortress Fortress;
        public readonly List<Character> AttackersGroup;

        public AttackTheFortress(Fortress fortress, List<Character> attackersGroup)
        {
            Fortress=fortress;
            AttackersGroup=attackersGroup;
        }
    }
}
