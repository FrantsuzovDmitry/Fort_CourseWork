using Assets.Scripts;
using Assets.Scripts.Managers;
using UnityEngine;

// Entry point
public class EntryPoint : MonoBehaviour
{
	private static Mediator _mediator;

	void Start()
    {
        InitializeComponents.ExecuteInitialization();
        _mediator = InitializeComponents.Mediator;

        // Entry point
        _mediator.StartFirstRound(Constants.MIN_PLAYER_ID);
    }
}
