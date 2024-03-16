using UnityEngine;

[System.Serializable]
public class Card
{
    public byte OwnerID;
    public readonly string cardName;
    public readonly Sprite logo;

    public Card(string cardName, Sprite logo)
    {
        this.cardName=cardName;
        this.logo=logo;
    }

    /// <summary>
    /// Return the 1 if card should to be selected, 0 otherwise
    /// </summary>
    public virtual NeedToBeSelected ProcessOnClick(in CardController cardController) { /*throw new System.Exception("Abstract method");*/ return NeedToBeSelected.NO; } // abstract

    public bool IsCardOnTheTable() { return this is Fortress || this is Sandglass || this is Rule; }

    public virtual void InvokeOnCardAppearsEvent() { }
}