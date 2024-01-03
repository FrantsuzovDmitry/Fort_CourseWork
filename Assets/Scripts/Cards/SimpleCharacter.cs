using UnityEngine;

[System.Serializable]
public class SimpleCharacter : Character
{
	public virtual int Force { get; }

	public SimpleCharacter() : base() { }

	public SimpleCharacter(SimpleCharacter card) : base(card)
	{
		this.Force = card.Force;
	}

	public SimpleCharacter(int force, Sprite illustration) : base("SimpleCharacter", illustration)
	{
		this.Force = force;
	}

	public SimpleCharacter(string cardName, Sprite illustration) : base(cardName, illustration) { }

	public override void EnterInGroup(ref int totalForce, ref int totalWeight)
	{
		totalForce += Force;
		totalWeight += 1;
	}
}
