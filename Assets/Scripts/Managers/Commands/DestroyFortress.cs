namespace Assets.Scripts.Managers.Commands
{
    public class DestroyFortress : Command
    {
        private Fortress fortressToDestroy;

        public DestroyFortress(Fortress fortressToDestroy, byte attackerID)
        {
            this.fortressToDestroy=fortressToDestroy;
        }

        public override void Execute()
        {
            ReturnDefendersToOwner();
            DestroyTheFortress();
        }

        private void ReturnDefendersToOwner()
        {
            var fortressOwnerID = _fortressManager.GetFortressOwner(fortressToDestroy.Rate);
            new ReturnUserCardsToHand(fortressOwnerID, fortressToDestroy.DefendersGroup.ToList())
                .Execute();
        }

        private void DestroyTheFortress()
        {
            _fortressManager.OnFortressDestroyed(fortressToDestroy);
            _cardVisualizationManager.OnFortressDestroyed(fortressToDestroy);
        }
    }
}
