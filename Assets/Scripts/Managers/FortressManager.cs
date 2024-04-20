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
	public Fortress Fortress(byte rate) => Fortresses[rate - 1];

	private Fortress[] Fortresses = new Fortress[8];
    private Mediator _mediator;

	public FortressManager(Mediator mediator)
	{
		_mediator = mediator;
	}

    public byte GetFortressOwner(byte fortressRate)
	{
		return FortressOwnerPairs[fortressRate];
	}

	public void ProcessAttackToFortress(Fortress defendingFort, GroupOfCharacters attackersGroup, byte attackerID)
	{
		var defendersGroup = defendingFort.DefendersGroup;
		int attackerForce = attackersGroup.TotalForce;
		int defendersForce = defendersGroup?.TotalForce ?? int.MinValue;

		if (attackerForce > defendersForce)
		{
			_mediator.OnFortressCaptured(defendingFort);

			FortressOwnerPairs[defendingFort.Rate] = attackerID;
			defendingFort.SetDefenders(attackersGroup);
			Debug.Log("Successful attack: TotalForce = " + attackerForce);
		}
		else if (attackerForce == defendersForce)
		{
			_mediator.OnFortressDestroyed(defendingFort.Rate);
		}
		else
		{
			_mediator.OnFortressUnsuccessfulAttacked(defendingFort.Rate);
		}
	}

	public void RemoveFortress(byte fortRate)
	{
		Fortresses[fortRate] = null;
		FortressOwnerPairs[fortRate] = NOT_A_PLAYER_ID;
	}

	public void AddNewFort(Fortress fort)
	{
		Fortresses[fort.Rate - 1] = fort;
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
