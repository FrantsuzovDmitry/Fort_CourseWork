using UnityEngine;

[System.Serializable]
public class Card
{
	public int ownerID;
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
		this.ownerID = card.ownerID;
		this.cardName = card.cardName;
        this.illustration = card.illustration;
    }
}
