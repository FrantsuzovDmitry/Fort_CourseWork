using UnityEngine;

[System.Serializable]
public class SimpleCharacter : Character
{
	public int Force { get; }

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
