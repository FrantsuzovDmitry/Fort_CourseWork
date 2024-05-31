using Assets.Scripts.AI.AIActions;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.AI
{
    public class GameInterface
    {
        private Mediator _mediator;

        public void Init(Mediator mediator)
        {
            _mediator = mediator;
        }

        public void NotifyAIAboutTurn(AIPlayer receiver)
        {
            // TODO: сделать преобразование информации.
            GameInfo gameInfo = new GameInfo();
            var aiAction = receiver.MakeDecision(gameInfo);
            HandleAIAction(aiAction);
        }

        public void OnRoundEnded(List<AIPlayer> AIPlayers)
        {
            AIPlayers.ForEach(player => ManipulateAIScrore.ManipulateScore(player));
           
        }

        private void HandleAIAction(AIAction action)
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

            _mediator.OnAttackStarted(action.Fortress);
            action.AttackersGroup.ForEach(attacker =>
            {
                _mediator.AddCharacterToGroup(attacker);
            });
            _mediator.OnFortressTriedAttacked();
        }

        private void GiveCardToAnotherPlayer(GiveCardToAnotherPlayer action)
        {

        }
    }
}
