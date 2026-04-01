using UnityEngine;

public enum StatType
{
    Happiness,
    Health,
    Energy,
    Money
}

[System.Serializable]
public class StoryChoice
{
    public string title;

    public int happinessChange;
    public int healthChange;
    public int energyChange;
    public int moneyChange;
    public string moneyRange;
    public Sprite frontImage;

    public string resultText;

    public string setFlag;
    public bool setFlagValue;
    public string addIntKey;
    public int addIntValue;

    public int nextNodeIndex;
}

[System.Serializable]
public class StoryNode
{
    public string id;
    public Sprite background;

    [TextArea(2, 6)]
    public string[] lines;

    public bool hasChoices;
    public StoryChoice[] choices;

    public int nextNodeIndex;

    public bool isCalendarNode;
    public Sprite calendarSprite;

    public bool hasCondition;
    public string conditionFlag;
    public int trueNodeIndex;
    public int falseNodeIndex;

    public bool hasStatCondition;
    public StatType statType;
    public int lowValue;
    public int highValue;
    public int lowNodeIndex;
    public int midNodeIndex;
    public int highNodeIndex;
}