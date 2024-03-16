using Assets.Scripts;
using UnityEngine;

// Entry point
public class EntryPoint : MonoBehaviour
{
	Mediator _mediator;
	void Start()
    {
		_mediator = new Mediator();
		_mediator.InitializeComponents();
		UIManager.instance.Init(_mediator);

		// Entry point
		_mediator.StartFirstRound(Constants.MIN_PLAYER_ID);
    }
}
