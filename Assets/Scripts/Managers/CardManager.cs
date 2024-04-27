using Assets.Scripts.GameEntities;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Managers
{
    public class CardManager
    {
        private readonly List<LinkedList<Card>> playersHands = new(4);
        private readonly CurrentDeck currentDeck;
        private readonly List<Card> sandglasses = new(3);

        public int NumberOfCardsInDeck => currentDeck.Count;

        public CardManager()
        {
            currentDeck = new CurrentDeck(new MainDeck());
            playersHands.Clear();

            for (int i = 0; i < Constants.MAX_PLAYER_ID + 1; i++)
            {
                playersHands.Add(new LinkedList<Card>());
            }
        }

        public Card GetCardFromDeck()
        {
            var card = currentDeck.Pop();

            if (!card.CardShouldBeOnTheTable())
                playersHands[TurnManager.instance.CurrentPlayerTurn].AddLast(card);

            if (card is Sandglass) sandglasses.Add(card);

            return card;
        }

        public List<Character> GetUserHandCharacters(byte playerID)
        {
            var list = playersHands[playerID].Where(c => c is Character).ToList();
            var list2 = list.Cast<Character>().ToList();

            return list2;
        }

        public void ChangeCardOwner(Character character, byte newOwnerID)
        {
            playersHands[character.OwnerID].Remove(character);
            character.OwnerID = newOwnerID;
            playersHands[newOwnerID].AddLast(character);
        }

        public void OnFortressCaptured(byte attackerID, List<Character> attackersGroup)
        {
            attackersGroup.ForEach(
                attackerCard => playersHands[attackerID].Remove(attackerCard));
        }

        public void OnNewRoundStarted(List<Card> winnersCardsThatShouldToRemove)
        {
            GenerateNewDeck(winnersCardsThatShouldToRemove);
            RemovePlayesCardsFromGame();
            RemoveSandglassesFromGame();
        }

        private void GenerateNewDeck(List<Card> cardsToRemove)
        {
            currentDeck.RemoveCardsFromDeck(cardsToRemove);
            ReturnOtherCardsInDeck();
            currentDeck.AddFiveCardFromMainDeck();
            currentDeck.Shuffle();
            currentDeck.DepersonalizeCards();
        }

        private void ReturnOtherCardsInDeck()
        {
            foreach (var hand in playersHands)
                foreach (var card in hand)
                    currentDeck.Push(card);

            // Returning sandglasses
            foreach (var card in sandglasses)
                currentDeck.Push(card);
        }

        private void RemovePlayesCardsFromGame() => playersHands.ForEach(hand => hand.Clear());

        private void RemoveSandglassesFromGame() => sandglasses.Clear();
    }
}