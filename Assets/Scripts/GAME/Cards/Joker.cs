using UnityEngine;

public class Joker : Character
{
    public Joker(Sprite illustration) : base("Joker", illustration) { }

    public override void EnterInGroup(ref int totalForce, ref int totalWeight)
    {
        if (totalForce == 0)
            return;
        else
        {
            int singleCharacterForce = totalForce / totalWeight;
            totalWeight++;
            totalForce += singleCharacterForce;
        }
    }
}
