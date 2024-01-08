using Assets.Scripts.Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class Fortress : Card
{
    public int Rate { get; }
    // TODO: заменить на groupOfCharacters
    public List<Character> DefendersGroup {  get; private set; }

    public Fortress() { }

    public Fortress(int rate, Sprite illustrarion) : base("Fortress", illustrarion)
    {
        this.Rate = rate;
    }

    public Fortress(Fortress fort) : base(fort)
    {
        this.Rate = fort.Rate;
        DefendersGroup = null;
    }

    public void SetDefenders(List<Character> defenders)
    {
        DefendersGroup = defenders;
    }

    // TODO: Use this in FortressManager
    public virtual bool IsRequirementsToDefendersAreAccept(GroupOfCharacters groupOfCharacters)
    {
        var characters = groupOfCharacters.SimpleCharacters;
		int comparisonForce = characters[0].Force;
		for (int i = 1; i < characters.Count; i++)
			if (comparisonForce != characters[i].Force)
				return false;
		return true;
    }
}
