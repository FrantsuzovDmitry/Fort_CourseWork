namespace Assets.Scripts.AI.AIActions
{
    public class AttackTheFortress : AIAction
    {
        public readonly byte FortressRate;

        public AttackTheFortress(byte fortressRate)
        {
            FortressRate=fortressRate;
        }
    }
}
