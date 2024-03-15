using Assets.Scripts;
using UnityEngine;

// Entry point
public class EntryPoint : MonoBehaviour
{
	void Start()
    {
		Mediator.InitializeComponents();

		// Entry point
		Mediator.StartFirstRound(Constants.MIN_PLAYER_ID);
    }
}
