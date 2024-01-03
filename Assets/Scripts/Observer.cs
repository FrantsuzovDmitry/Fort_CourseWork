using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Observer
{
    public delegate void OnFortressAttacked(CardController cardController);
    public static OnFortressAttacked onFortressAttacked;

    public delegate void OnFortressCaptured(CardController cardController);
    public static OnFortressCaptured onFortressCaptured;

    public delegate void OnGameStopped();
    public static OnGameStopped onGameStopped;

	public delegate void OnCardTaken();
	public static OnGameStopped onCardTaken;

	public delegate void OnAttackStopped();
	public static OnGameStopped onAttackStopped;
}