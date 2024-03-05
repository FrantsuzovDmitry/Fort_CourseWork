using UnityEngine;

[System.Serializable]
public class Mirror : Character
{
	public Mirror(Sprite illustration = null) : base("Mirror", illustration) { }

	public override void EnterInGroup(ref int totalForce, ref int totalWeight)
	{
		totalForce *= 2;
	}
}
