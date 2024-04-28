using UnityEngine;

[System.Serializable]
public class Rule : Card
{
    public Rule(Sprite illustration) : base("Rule", illustration) { }

    public override void InvokeOnCardAppearsEvent()
    {
        Mediator.OnRuleAppears(this);
    }
}
