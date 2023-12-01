using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Character
{
    public Joker(Sprite logo) : base("Joker", logo) { }

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
