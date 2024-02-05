using UnityEngine;

[System.Serializable]
public class Sandglass : Card
{
    public Sandglass(Sprite logo) : base("Sandglass", logo) { }

	public override NeedToBeSelected ProcessOnClick(in CardController c)
	{
		if (CurrentUserStateController.NowTheProcessOfCreatingGroupIsUnderway)
			Mediator.OnAttackStopped();
		return NeedToBeSelected.NO;
	}
}
