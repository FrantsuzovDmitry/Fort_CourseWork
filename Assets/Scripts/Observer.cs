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
}