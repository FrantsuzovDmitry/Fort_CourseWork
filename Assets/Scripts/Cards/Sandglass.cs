using UnityEngine;

[System.Serializable]
public class Sandglass : Card
{
    public Sandglass(Sprite illustration) : base("Sandglass", illustration) { }

	public override NeedToBeSelected ProcessOnClick(in CardController c)
	{
		if (CurrentUserIntentionState.IsCreatingGroupInProgress)
			Mediator.OnAttackStopped();
		return NeedToBeSelected.NO;
	}

    public override void InvokeOnCardAppearsEvent()
    {
		Mediator.OnSandglassAppears();
    }
}
