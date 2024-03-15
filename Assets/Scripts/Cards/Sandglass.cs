using UnityEngine;

[System.Serializable]
public class Sandglass : Card
{
    public Sandglass(Sprite logo=null) : base("Sandglass", logo) { }

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
