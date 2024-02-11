using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Card
{
	public byte OwnerID;
	public string cardName;
    public Sprite illustration;

    public Card() { }

    public Card(string cardName, Sprite illustration)
    {
        this.cardName=cardName;
        this.illustration=illustration;
    }

    public Card(Card card)
    {
		this.OwnerID = card.OwnerID;
		this.cardName = card.cardName;
        this.illustration = card.illustration;
    }

    /// <summary>
    /// Return the 1 if card should to be selected, 0 otherwise
    /// </summary>
    public virtual NeedToBeSelected ProcessOnClick(in CardController cardController) { /*throw new System.Exception("Abstract method");*/ return NeedToBeSelected.NO; } // abstract
}