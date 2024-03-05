using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Card
{
	public byte OwnerID;
	public string cardName;
    public Sprite logo;

    public Card() { }

    public Card(string cardName, Sprite logo = null)
    {
        this.cardName=cardName;
        this.logo=logo;
    }

    public Card(Card card)
    {
		this.OwnerID = card.OwnerID;
		this.cardName = card.cardName;
        this.logo = card.logo;
    }

    public void SetLogo(Sprite logo)
    {
        this.logo = logo;
    }

    /// <summary>
    /// Return the 1 if card should to be selected, 0 otherwise
    /// </summary>
    public virtual NeedToBeSelected ProcessOnClick(in CardController cardController) { /*throw new System.Exception("Abstract method");*/ return NeedToBeSelected.NO; } // abstract

    public bool IsCardOnTheTable() { return this is Fortress || this is Sandglass || this is Rule; }

    public virtual void InvokeOnCardAppearsEvent() { }
}