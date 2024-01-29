using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUserStateController : MonoBehaviour
{
    private GroupOfCharacters groupOfCharacters;
    private bool nowTheProcessOfCreatingGroupIsUnderway;
    private byte selectedFortToAttack;

	void Start()
    {
		nowTheProcessOfCreatingGroupIsUnderway = false;
    }

	public void StartCreatingOfGroupOfCharacters()
	{
		Debug.Log("Start creating");
		nowTheProcessOfCreatingGroupIsUnderway = true;
	}

	public void StopCreatingOfGroupOfCharacters()
	{
		Debug.Log("Stop creating");
		nowTheProcessOfCreatingGroupIsUnderway = false;
	}
}
