using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FortressManager : MonoBehaviour
{
    public static FortressManager instance;
    public Dictionary<Player, List<Fortress>> capturedFortresses = new();
    public List<Fortress> notCapturedFortress = new List<Fortress>();

    private void Awake()
    {
        instance = this;
    }

    private int CalculateForce(Character[] groupOfCharacters)
    {
        // sorting the character in group, Mirrors and Jokers must be in the end;
        var begIndex = 0;
        var endIndex = groupOfCharacters.Length;
        Character[] tempGroup = new Character[groupOfCharacters.Length];
        for (int i = 0; i < groupOfCharacters.Length; i++)
        {
            if (groupOfCharacters[i] is Mirror || groupOfCharacters[i] is Joker)
            {
                tempGroup[endIndex - 1] = groupOfCharacters[i];
                --endIndex;
            }
            else
            {
                tempGroup[begIndex] = groupOfCharacters[i];
                ++begIndex;
            }
        }

        int totalForce = 0, totalWeight = 0;
        foreach (SimpleCharacter character in tempGroup)
            character.EnterInGroup(ref totalForce, ref totalWeight);

        return totalForce * totalWeight;    // Тут не совсем корректно, ибо зеркало умножает силу ДО того, как ее умножили на вес карт
    }

    public void AttackToFortress(CardController defendingFort)
    {
        int attackerID = TurnManager.instance.currentPlayerTurn;
        var fort = (Fortress)defendingFort.card;

        var attackersGroup = CardManager.instance.GroupOfCharacters;
        var defendersGroup = fort.defendersGroup;
        int attackerForce, defendersForce;

        if (defendersGroup != null)
        {
            attackerForce = CalculateForce(attackersGroup.ToArray());
            defendersForce = CalculateForce(defendersGroup.ToArray());
        }
        else
        {
            attackerForce = 10000;     // any number greater than 0
            defendersForce = 0;
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

        TurnManager.instance.onAttackStopped?.Invoke();
    }

    private void RemoveFortressFromList(List<Character> attackersGroup, Player defender, Fortress fort)
    {
        if (fort.defendersGroup == null)
        {
            // If there are no defenders
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
        //CardManager.instance.onFortAdded += AddFort;
        Observer.onFortressAttacked += AttackToFortress;
    }

    public void AddFort(Card fort)
    {
        notCapturedFortress.Add((Fortress)fort);
    }
}
