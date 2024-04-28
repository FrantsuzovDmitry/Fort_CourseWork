using Assets.Scripts.AI.AIActions;
using System;

namespace Assets.Scripts.AI
{
    public class GameInterface
    {
        private Mediator _mediator;

        public void Init(Mediator mediator)
        {
            _mediator = mediator;
        }

        public void NotifyAIAboutTurn()
        {

        }

        public void HandleAIActrion(AIAction action)
        {
            switch (action)
            {
                case TakeCard _:
                    TakeCard();
                    break;
                case AttackTheFortress _:
                    AttackTheFortress(action as AttackTheFortress);
                    break;
                case GiveCardToAnotherPlayer _:
                    GiveCardToAnotherPlayer(action as GiveCardToAnotherPlayer);
                    break;
                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }

        private void TakeCard()
        {
            _mediator.OnCardTaken();
        }

        private void AttackTheFortress(AttackTheFortress action)
        {

        }

        private void GiveCardToAnotherPlayer(GiveCardToAnotherPlayer action)
        {

        }
    }
}
