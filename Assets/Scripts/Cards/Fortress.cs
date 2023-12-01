using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fortress : Card
{
    public int rate;
    public List<Character> defendersGroup;

    public Fortress() { }

    public Fortress(int rate, Sprite illustrarion) : base("Fortress", illustrarion)
    {
        this.rate = rate;
    }

    public Fortress(Fortress fort) : base(fort)
    {
        this.rate = fort.rate;
        defendersGroup = null;
    }

    public void SetDefenders(List<Character> defenders)
    {
        defendersGroup = defenders;
    }
}
