using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardTemplate
{
    public string ClassName;
    public byte force;
    public byte weight;
    public byte rate;

    public CardTemplate(string name, byte force, byte weight, byte rate)
    {
        this.ClassName = name;
        this.force = force;
        this.weight = weight;
        this.rate = rate;
    }
}

public static class CardDatabase
{
    public const int NumberOfCards = 15;

    public static CardTemplate[] CardTemplates = new CardTemplate[]
    {
        new CardTemplate("Rule",        0, 0, 0),
        new CardTemplate("Fort",        0, 0, 3),
        new CardTemplate("Character",   1, 1, 0),
        new CardTemplate("Character",   2, 1, 0),
        new CardTemplate("Character",   1, 1, 0),
        new CardTemplate("Character",   2, 1, 0),
        new CardTemplate("Rule",        0, 0, 0),
        new CardTemplate("Rule",        0, 0, 0),
        new CardTemplate("Character",   3, 1, 0),
        new CardTemplate("Sandglasses", 0, 0, 0),   // 10 cards here
    };
}