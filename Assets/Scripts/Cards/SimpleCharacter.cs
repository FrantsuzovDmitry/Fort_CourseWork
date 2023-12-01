using UnityEngine;

[System.Serializable]
public class SimpleCharacter : Character
{
    public int force, weight;

    public SimpleCharacter() : base() { }

    public SimpleCharacter(SimpleCharacter card) : base(card)
    {
        this.force = card.force;
        this.weight = card.weight;
    }

    // Можно удалить OwnerID
    public SimpleCharacter(int force, int weight, int ownerID, Sprite illustration) : base("SimpleCharacter", illustration)
    {
        this.force = force;
        this.weight = weight;
    }

    public SimpleCharacter(string cardName, Sprite illustration) : base(cardName, illustration) { }

    public override void EnterInGroup(ref int totalForce, ref int totalWeight)
    {
        totalForce += force;
        totalWeight += weight;
    }
}
