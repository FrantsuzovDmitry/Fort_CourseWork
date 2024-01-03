using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : Card
{
    public bool IsInGroup = false;

    public Character() : base() { }

    public Character(Character card) : base(card) { }

    public Character(string cardName, Sprite illustration) : base(cardName, illustration) { }

    public virtual void EnterInGroup(ref int totalForce, ref int totalWeight) { }
}
