namespace Assets.Scripts.AI.AIActions
{
    public class GiveCardToAnotherPlayer : AIAction
    {
        public readonly Card SelectedCard;

        public GiveCardToAnotherPlayer(Card selectedCard)
        {
            SelectedCard=selectedCard;
        }
    }
}
