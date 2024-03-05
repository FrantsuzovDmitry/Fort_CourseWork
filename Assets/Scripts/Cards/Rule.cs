using UnityEngine;

[System.Serializable]
public class Rule : Card
{
    public Rule(Sprite logo=null) : base("Rule", logo) { }

    public override void InvokeOnCardAppearsEvent()
    {
        Mediator.OnRuleAppears(this);
    }
}
