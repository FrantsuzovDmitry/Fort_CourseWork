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

        Make20Turns();
    }

    private void Make20Turns()
    {
        for (int i = 0; i < 20; i++)
        {
            _mediator.OnCardTaken();
            _mediator.OnTurnEnded();
        }
    }
}
