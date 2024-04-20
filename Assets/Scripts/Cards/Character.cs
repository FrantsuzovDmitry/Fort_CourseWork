using UnityEngine;

[System.Serializable]
public class Character : Card
{
    public bool IsInGroup = false;

    public Character(string cardName, Sprite illustration) : base(cardName, illustration) { }

    public virtual void EnterInGroup(ref int totalForce, ref int totalWeight) { }

    public override NeedToBeSelected ProcessOnClick(in CardController c)
    {
        if (Mediator.IsSelectingCardToGiveInProgress)
        {
            if (c.IsOneOfTheCardsThatCanBeGiven())
            {
                Mediator.OnCardToGiveChosen(this);
                return NeedToBeSelected.YES;
            }
            return NeedToBeSelected.NO;
        }

        if (Mediator.IsCreatingGroupInProgress)
        {
            if (!c.IsCardInPlayerHand())
                return NeedToBeSelected.NO;

            if (this.IsInGroup)
            {
                IsInGroup = false;
                Mediator.RemoveCharacterFromGroup(this);
                return NeedToBeSelected.NO;
            }

            IsInGroup = true;
            Mediator.AddCharacterToGroup(this);
            return NeedToBeSelected.YES;
        }
        return NeedToBeSelected.NO;
    }
}