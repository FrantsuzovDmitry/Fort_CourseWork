using Assets.Scripts;
using Assets.Scripts.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class Fortress : Card
{
    public byte Rate { get; }
    public GroupOfCharacters DefendersGroup { get; private set; }

    public Fortress(byte rate, Sprite illustrarion=null) : base("Fortress", illustrarion)
    {
        this.Rate = rate;
    }

    public void SetDefenders(GroupOfCharacters defenders)
    {
        if (defenders != null)
            DefendersGroup = defenders;
        else
            throw new Exception("Trying to set nullable defenders group to the fort");
    }

    public virtual bool IsRequirementsToDefendersAreAccept(GroupOfCharacters groupOfCharacters)
    {
        var characters = groupOfCharacters.SimpleCharacters;
		int comparisonForce = characters[0].Force;
		for (int i = 1; i < characters.Count; i++)
			if (comparisonForce != characters[i].Force)
				return false;
		return true;
    }

	public override NeedToBeSelected ProcessOnClick(in CardController c)
	{
        if (c.IsCardIsPlayersOwn())
        {
			Mediator.OnAttackStopped();
			// TODO: Show the defenders group
			// ShowDefendersGroup()
		}
        else
        {
            Mediator.OnAttackStopped();
			Mediator.OnAttackStarted(Rate);
		}
        
        return NeedToBeSelected.YES;
	}

    public override void InvokeOnCardAppearsEvent()
    {
        Mediator.OnFortressAppears(this);
    }
}
