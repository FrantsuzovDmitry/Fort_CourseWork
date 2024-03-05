using Assets.Scripts;
using UnityEngine;

// Entry point
public class GameActionManager : MonoBehaviour
{
    public static GameActionManager instance;

	private void Awake()
	{
		instance = this;
	}

	void Start()
    {
		Mediator.InitializeComponents();

		// Entry point
		Mediator.StartFirstRound(Constants.MIN_PLAYER_ID);
		//Mediator.StartNewRound(Constants.MIN_PLAYER_ID);
    }
}
