using Assets.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Constants;

/// <summary>
/// Responsible for keeping records of forts in the game
/// </summary>
public class FortressManager : MonoBehaviour
{
	public Dictionary<byte, byte> FortressOwnerPairs = new(MAX_FORT_RATE);       // Pairs FortressRate <-> owner
	public Fortress[] Fortresses = new Fortress[8];

    private Mediator _mediator;

	public FortressManager(Mediator mediator)
	{
		_mediator = mediator;
	}

    public byte GetFortressOwner(byte fortressRate)
	{
		return FortressOwnerPairs[fortressRate];
	}

	public void ProcessAttackToFortress(byte defendingFortRate, GroupOfCharacters attackersGroup, byte attackerID)
	{
		var defendingFort = Fortresses.FirstOrDefault(f => f.Rate == defendingFortRate);
		if (defendingFort == null) throw new Exception("Trying to attack unexisting fortress");
		if (attackersGroup.Characters.Count < 1) throw new Exception("Empty attackers group");

		var defendersGroup = defendingFort.DefendersGroup;
		int attackerForce = attackersGroup.TotalForce;
		int defendersForce = defendersGroup?.TotalForce ?? int.MinValue;

		if (attackerForce > defendersForce)
		{
			_mediator.OnFortressCaptured(defendingFort);

			FortressOwnerPairs[defendingFortRate] = attackerID;
			defendingFort.SetDefenders(attackersGroup);
			Debug.Log("Successful attack: TotalForce = " + attackerForce);
		}
		else if (attackerForce == defendersForce)
		{
			_mediator.OnFortressDestroyed(defendingFort);
		}
		else
		{
			_mediator.OnFortressUnsuccessfulAttacked(defendingFortRate);
		}
	}

	public void RemoveFortress(Fortress fort)
	{
		Fortresses[fort.Rate] = null;
		FortressOwnerPairs[fort.Rate] = NOT_A_PLAYER_ID;
	}

	public void AddNewFort(Fortress fort)
	{
		Fortresses[fort.Rate] = fort;
	}

    public List<Fortress> GetPlayersForts(byte lastWinnerID)
    {
        List<Fortress> result = new List<Fortress>(FortressOwnerPairs.Count);
		foreach (var pair in FortressOwnerPairs)
		{
			if (pair.Value ==  lastWinnerID)
				result.Add(Fortresses[pair.Key - 1]);
		}	

		return result;
    }
}
