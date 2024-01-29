using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Отвечает за учет фортов в игре
/// </summary>
public class FortressManager : MonoBehaviour
{
    public static FortressManager instance;
    public Dictionary<Player, List<Fortress>> capturedFortresses = new();
    public List<Fortress> notCapturedFortress = new List<Fortress>();

    private void Awake()
    {
        instance = this;
    }

	/// Probably move to GameActionManager
    /////////////////////////////////////////////////////////////////////
	private int CalculateForce(List<Character> groupOfCharacters)
    {
        // sorting the character in group, Mirrors and Jokers must be in the end;

        List<Character> simpleCharacters = new List<Character>();
        List<Character> mirrors = new List<Character>(3);
        List<Character> jokers = new List<Character>(3);

		#region Sort the characters in the group
		foreach (Character character in groupOfCharacters)
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

	/// Probably move to GameActionManager
	/////////////////////////////////////////////////////////////////////
    // TODO: Refactor this method
	public void ProcessAttackToFortress(CardController defendingFort)
    {
        int attackerID = TurnManager.instance.currentPlayerTurn;
        var fort = (Fortress)defendingFort.card;

        var attackersGroup = CardManager.instance.GroupOfCharacters;
        var defendersGroup = fort.DefendersGroup;
        int attackerForce, defendersForce;

        if (attackersGroup.Count < 1) return;

        if (defendersGroup != null)
        {
            attackerForce = CalculateForce(attackersGroup);
            defendersForce = CalculateForce(defendersGroup);
        }
        else
        {
            attackerForce = int.MaxValue;
            defendersForce = int.MinValue;
        }

        if (attackerForce > defendersForce)
        {
            // Successful capture
            Player attacker = PlayerManager.instance.FindPlayerByID(attackerID);
            Player defender = PlayerManager.instance.FindPlayerByID(fort.ownerID);

            RemoveFortressFromList(attackersGroup, defender, fort);

            // Adding new Fort to the player collection
            if (capturedFortresses.ContainsKey(attacker))
            {
                capturedFortresses[attacker].Add(fort);
            }
            // Creating of new pair Player - Fort
            else
            {
                capturedFortresses.Add(attacker, new List<Fortress> { fort });
            }

            CardManager.instance.ChangeParentPosition(defendingFort);
            CardManager.instance.RemoveAttackersFromHand();

            Debug.Log("Successful attack: TotalForce = " + attackerForce);
        }
        else
        {
            // Unsuccessful capture
            Debug.Log("Unsuccessful attack");
        }

        Observer.onAttackStopped();
        //TurnManager.instance.onAttackStopped?.Invoke();
    }

    private void RemoveFortressFromList(List<Character> attackersGroup, Player defender, Fortress fort)
    {
        if (fort.DefendersGroup == null)
        {
            notCapturedFortress.
                Find(x => x == fort).SetDefenders(attackersGroup);
            notCapturedFortress.Remove(fort);
        }
        else
        {
            capturedFortresses[defender].
                Find(x => x == fort).SetDefenders(attackersGroup);
            capturedFortresses[defender].Remove(fort);
        }
    }

    // Called when the Unity-object is active
    private void OnEnable()
    {
        // Subscribe to action
        Observer.onFortressAttacked += ProcessAttackToFortress;
    }

    public void AddFortToList(Card fort)
    {
        notCapturedFortress.Add((Fortress)fort);
    }
}
