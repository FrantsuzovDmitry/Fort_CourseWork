using UnityEngine;

[System.Serializable]
public class Mirror : Character
{
    public Mirror(Sprite illustration) : base("Mirror", illustration) { }

    public override void EnterInGroup(ref int totalForce, ref int totalWeight)
    {
        if (totalForce == 0)
            return;
        else
            totalForce *= 2;
    }
}
