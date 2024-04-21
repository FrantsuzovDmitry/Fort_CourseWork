using Assets.Scripts.Cards;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Constants;

/// <summary>
/// Responsible for keeping records of forts in the game
/// </summary>
public class FortressManager
{
    public Dictionary<byte, byte> FortressOwnerPairs = new(MAX_FORT_RATE);       // Pairs FortressRate <-> owner

    private Fortress[] _fortresses = new Fortress[8];
    private Mediator _mediator;

    public void Init(Mediator mediator)
    {
        _mediator = mediator;
    }

    public byte GetFortressOwner(byte fortressRate)
    {
        return FortressOwnerPairs[fortressRate];
    }

    public void ProcessAttackingToFortress(Fortress defendingFort, GroupOfCharacters attackersGroup, byte attackerID)
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
            _mediator.OnFortressDestroyed(defendingFort);
        }
        else
        {
            _mediator.OnFortressUnsuccessfulAttacked(defendingFort.Rate);
        }
    }

    public void OnFortressDestroyed(Fortress fort)
    {
        _fortresses[fort.Rate] = null;
        FortressOwnerPairs[fort.Rate] = NOT_A_PLAYER_ID;
    }

    public void AddNewFort(Fortress fort)
    {
        _fortresses[fort.Rate - 1] = fort;
    }

    public List<Fortress> GetPlayersForts(byte lastWinnerID)
    {
        List<Fortress> result = new List<Fortress>(FortressOwnerPairs.Count);
        foreach (var pair in FortressOwnerPairs)
        {
            if (pair.Value ==  lastWinnerID)
                result.Add(_fortresses[pair.Key - 1]);
        }

        return result;
    }
}
