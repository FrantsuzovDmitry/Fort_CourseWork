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
    public static FortressManager instance;
    public Dictionary<byte, byte> FortressesOwnersPairs = new(MAX_FORT_RATE);       // Pairs Fortress <-> owner
    public Fortress[] Fortresses = new Fortress[8];

	private void Awake()
    {
        instance = this;
    }

	private int CalculateGroupForce(GroupOfCharacters groupOfCharacters)
    {
        // sorting the character in group, Mirrors and Jokers must be in the end;

        List<Character> simpleCharacters = new List<Character>();
        List<Character> mirrors = new List<Character>(3);
        List<Character> jokers = new List<Character>(3);

		#region Sort the characters in the group
		foreach (Character character in groupOfCharacters.Characters)
        {
            if (character is SimpleCharacter)
                simpleCharacters.Add(character);
            else if (character is Joker)
                jokers.Add(character);
            else mirrors.Add(character);
        }
        simpleCharacters.AddRange(jokers);
        simpleCharacters.AddRange(mirrors);
		#endregion

		int totalCurrentForce = 0, totalWeight = 0;
        // Calculate total force of characters and jokers
        foreach (Character character in simpleCharacters)
            character.EnterInGroup(ref totalCurrentForce, ref totalWeight);

        return totalCurrentForce * totalWeight;    // Тут не совсем корректно, ибо зеркало умножает силу ДО того, как ее умножили на вес карт
    }

	public void ProcessAttackToFortress(byte defendingFortRate, GroupOfCharacters attackersGroup, byte attackerID)
    {
        var defendingFort = Fortresses.FirstOrDefault(f => f.Rate == defendingFortRate);
        if (defendingFort == null) throw new Exception("Trying to attack unexisting fortress");
        if (attackersGroup.Characters.Count < 1) throw new Exception("Empty attackers group");

		var defendersGroup = defendingFort.DefendersGroup;
        int attackerForce, defendersForce;

        if (defendersGroup != null)
        {
            attackerForce = CalculateGroupForce(attackersGroup);
            defendersForce = CalculateGroupForce(defendersGroup);
        }
        else
        {
            attackerForce = int.MaxValue;
            defendersForce = int.MinValue;
        }
        
        if (attackerForce > defendersForce)         // Successful capturing
		{
            FortressesOwnersPairs[defendingFortRate] = attackerID;

            Mediator.OnFortressCaptured(defendingFort, attackerID);
			Debug.Log("Successful attack: TotalForce = " + attackerForce);
		}
        else if (attackerForce == defendersForce)   // Fortress has been destroyed
        {
            Mediator.OnFortressDestroyed(defendingFort);
        }
		else                                        // Unsuccessful capturing
		{
            Mediator.OnFortressUnsuccessfulAttacked();
        }
    }

    public void RemoveFortress(Fortress fort)
    {
        Fortresses[fort.Rate] = null;
        FortressesOwnersPairs[fort.Rate] = MIN_PLAYER_ID - 1;
    }

    public void AddNewFort(Fortress fort)
    {
        Fortresses[fort.Rate] = fort;
    }
}
